// Profile & Character Scripts

document.addEventListener("DOMContentLoaded", function () {
    // --- Avatar Preview ---
    const avatarInput = document.getElementById("avatarUpload");
    const avatarWrapper = document.getElementById("avatarWrapper");
    const modalImg = document.getElementById("modalAvatarImage");
    const fileLabel = document.querySelector('.custom-file-label');

    avatarInput.addEventListener("change", function () {
        if (this.files && this.files[0]) {
            const reader = new FileReader();
            reader.onload = function (e) {
                avatarWrapper.innerHTML = "";
                const img = document.createElement("img");
                img.src = e.target.result;
                img.alt = "New Avatar";
                img.classList.add("clickable-avatar", "avatar-image");
                img.id = "avatarPreview";
                img.onclick = openAvatarModal;
                avatarWrapper.appendChild(img);
                modalImg.src = e.target.result;
            };
            reader.readAsDataURL(this.files[0]);
            fileLabel.innerHTML = `<i class="fa-solid fa-scroll me-1"></i> ${this.files[0].name}`;
            stopRuneAnimation();
            const clockTick = document.getElementById("clockTickSound");
            clockTick.pause();
            clockTick.currentTime = 0;
        }
    });

    // --- Bio Character Count ---
    const bioTextarea = document.querySelector("textarea[asp-for='Input.Bio']");
    if (bioTextarea) {
        updateCharCount(bioTextarea);
        bioTextarea.addEventListener("input", function () { updateCharCount(this); });
    }

    // --- Bootstrap Dropdowns ---
    document.querySelectorAll('.dropdown-toggle').forEach(function (dropdown) {
        new bootstrap.Dropdown(dropdown);
    });
});

// --- Avatar Modal ---
function openAvatarModal() {
    const preview = document.getElementById("avatarPreview");
    if (preview) document.getElementById("modalAvatarImage").src = preview.src;
    document.getElementById("avatarModal").style.display = "block";
}

function closeAvatarModal(event) {
    if (event && event.target.id === "modalAvatarImage") return;
    document.getElementById("avatarModal").style.display = "none";
    stopRuneAnimation();
    const clockTick = document.getElementById("clockTickSound");
    clockTick.pause();
    clockTick.currentTime = 0;
}

// --- Password Modal ---
function openPasswordModal() {
    const passwordModal = document.getElementById("passwordModal");
    passwordModal.style.display = "block";
    const sound = document.getElementById("magicSound");
    sound.currentTime = 0;
    sound.play().catch(e => console.warn("Magic sound failed to play:", e));
    const clockTick = document.getElementById("clockTickSound");
    clockTick.currentTime = 0;
    clockTick.play().catch(e => console.warn("Clock tick failed to play:", e));
    startRuneAnimation();
}

function closePasswordModal() {
    const passwordModal = document.getElementById("passwordModal");
    passwordModal.style.display = "none";
    stopRuneAnimation();
    const clockTick = document.getElementById("clockTickSound");
    clockTick.pause();
    clockTick.currentTime = 0;
}

// --- Cover Modal ---
window.openCoverModal = function () {
    document.getElementById("coverModal").classList.add("active");
};

window.closeCoverModal = function () {
    document.getElementById("coverModal").classList.remove("active");
};

function selectCover(imagePath, isLocked) {
    if (isLocked) {
        alert("You must acquire the Retirement ending to unlock this cover.");
        return;
    }
    const selectedCoverPreview = document.getElementById("selectedCoverPreview");
    selectedCoverPreview.src = imagePath;
    selectedCoverPreview.style.display = "block";
    document.getElementById("selectedCoverUrl").value = imagePath;
    document.querySelectorAll('.cover-thumbnail').forEach(img => img.classList.remove('selected'));
    const selectedImg = [...document.querySelectorAll('.cover-thumbnail')]
        .find(img => img.getAttribute('data-cover') === imagePath);
    if (selectedImg) selectedImg.classList.add('selected');
}

window.saveUserCoverChoice = function () {
    document.getElementById('profileForm').submit();
};

// --- Tooltip for Locked Covers ---
function showTooltip() {
    const tooltip = document.getElementById("lockedTooltip");
    tooltip.style.display = "block";
    document.addEventListener("mousemove", positionTooltip);
    setTimeout(() => {
        tooltip.style.display = "none";
        document.removeEventListener("mousemove", positionTooltip);
    }, 2000);
}

function positionTooltip(event) {
    const tooltip = document.getElementById("lockedTooltip");
    tooltip.style.left = (event.pageX + 10) + "px";
    tooltip.style.top = (event.pageY + 10) + "px";
}

defaults
// --- Toggle Character Sheet ---
function toggleSheet(id) {
    const sheet = document.getElementById(`sheet-${id}`);
    sheet.style.display = sheet.style.display === 'block' ? 'none' : 'block';
}

// --- Bio Character Counter ---
function updateCharCount(textarea) {
    const count = textarea.value.length;
    const max = textarea.maxLength;
    const counter = document.getElementById("bioCharCount");
    counter.textContent = `${count} / ${max} characters used`;
    counter.style.color = count >= 200 ? "crimson"
        : count >= 190 ? "orange"
            : count >= 150 ? "gold"
                : "lightgreen";
}

// --- Rune Animation ---
let runeInterval;
function startRuneAnimation() {
    runeInterval = setInterval(() => { spawnRune(); spawnRune(); }, 300);
}

function spawnRune() {
    const runes = ["ᚠ", "ᛞ", "ᛃ", "ᚨ", "ᚱ", "ᚾ"];
    const colors = ["#4B0082", "#191970", "silver", "gold"];
    const runeEl = document.createElement("div");
    runeEl.classList.add("rune");
    runeEl.textContent = runes[Math.floor(Math.random() * runes.length)];
    runeEl.style.color = colors[Math.floor(Math.random() * colors.length)];
    runeEl.style.left = Math.random() * 95 + "%";
    document.getElementById("runeContainer").appendChild(runeEl);
    setTimeout(() => runeEl.remove(), 5500);
}

function stopRuneAnimation() {
    clearInterval(runeInterval);
    document.getElementById("runeContainer").innerHTML = "";
}

function startRuneAnimationAndPlayClock() {
    startRuneAnimation();
    const clockTick = document.getElementById("clockTickSound");
    clockTick.currentTime = 0;
    clockTick.play().catch(() => { });
}

// --- Level Up & Subclass Selection ---
// --- Level Up & Subclass Selection ---
function levelUpCharacter(charId) {
    const subclassNeededLevels = {
        barbarian: 3, bard: 3, cleric: 1, druid: 2,
        fighter: 3, monk: 3, paladin: 3, ranger: 2,
        rogue: 3, sorcerer: 1, warlock: 1, wizard: 2
    };
    const subclassOptions = window._subclassOptions || {};

    const classElem = document.querySelector(`#sheet-${charId} .character-class`);
    const levelElem = document.querySelector(`#sheet-${charId} .character-level`);
    if (!classElem || !levelElem) {
        console.warn("Can't find class or level element for charId", charId);
        return sendLevelRequest(charId, null);
    }

    const className = classElem.innerText.trim().toLowerCase();
    const currentLevel = parseInt(levelElem.innerText.replace("Level:", "").trim(), 10);
    const nextLevel = currentLevel + 1;

    // only prompt if this level grants a subclass
    if (subclassNeededLevels[className] === nextLevel) {
        const options = subclassOptions[className] || [];
        if (options.length) {
            // build your markup
            let html = `<div id="subclass-card-grid">`;
            options.forEach(opt => {
                html += `
          <div class="subclass-card mystic" data-value="${opt.value}">
            <img src="${opt.img}" class="mystic-img" alt="" />
            <h4 class="mystic-title">${opt.value}</h4>
            <p class="mystic-desc">${opt.desc}</p>
          </div>`;
            });
            html += `</div>`;

            Swal.fire({
                title: `Choose a Subclass for ${capitalize(className)}`,
                html,
                width: "60%",
                showCancelButton: true,
                cancelButtonText: "Cancel",
                confirmButtonText: "Confirm",
                customClass: {
                    popup: 'swal2-mystic',
                    htmlContainer: 'swal2-mystic-body'
                },
                didOpen: () => {
                    document.querySelectorAll(".subclass-card").forEach(card => {
                        card.addEventListener("click", () => {
                            document
                                .querySelectorAll(".subclass-card.selected")
                                .forEach(c => c.classList.remove("selected"));
                            card.classList.add("selected");
                        });
                    });
                },
                preConfirm: () => {
                    const sel = document.querySelector(".subclass-card.selected");
                    if (!sel) {
                        Swal.showValidationMessage("You must select one!");
                        return false;
                    }
                    return sel.dataset.value;
                }
            })
                .then(result => {
                    if (result.isConfirmed) {
                        sendLevelRequest(charId, result.value);
                    }
                });

            // IMPORTANT: stop here so fallback doesn't fire
            return;
        }
    }

    // fallback: no subclass needed (or none defined)
    sendLevelRequest(charId, null);
}



function sendLevelRequest(charId, subclass) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    fetch('/Character/SimpleLevelUp', { method: 'POST', headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': token }, body: JSON.stringify({ id: charId, subclass }) })
        .then(r => r.json()).then(data => data.success ? location.reload() : alert('Error leveling up'));
}

function capitalize(str) { return str.charAt(0).toUpperCase() + str.slice(1); }
