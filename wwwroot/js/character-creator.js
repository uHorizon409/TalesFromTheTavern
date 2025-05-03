// Dynamic Subclass Selector

// 1) Data: all subclass options
window._subclassOptions = {
    barbarian: [
        { value: "Path of the Berserker", img: "/images/subclasses/barbberserker-subclass.png", desc: "Unbridled rage that maximizes raw damage." },
        { value: "Path of Wild Magic", img: "/images/subclasses/barbwildmagic-subclass.png", desc: "Churns chaotic energies during each rage." },
        { value: "Path of The Giant", img: "/images/subclasses/barbgiant-subclass.png", desc: "Embrace the colossal might and ancient power of the giants." },
        { value: "Wildheart", img: "/images/subclasses/barbwildheart-subclass.png", desc: "Channel the primal fury and untamed spirit of nature." }
    ],
    bard: [
        { value: "College of Lore", img: "/images/subclasses/bardlore-subclass.png", desc: "Scholars of wide knowledge and magical secrets." },
        { value: "College of Valor", img: "/images/subclasses/bardvalour-subclass.png", desc: "Inspires martial prowess in allies." },
        { value: "College of Glamour", img: "/images/subclasses/bardglamour-subclass.png", desc: "Enchanters who harness the Feywild’s beauty and magic." },
        { value: "College of Swords", img: "/images/subclasses/bardsword-subclass.png", desc: "Blends artistry with swordplay and blade flourishes." }
    ],
    cleric: [
        { value: "Life Domain", img: "/images/subclasses/clericlife-subclass.png", desc: "Embodies healing and the defense of life." },
        { value: "Light Domain", img: "/images/subclasses/clericlight-subclass.png", desc: "Channels radiant power to banish darkness." },
        { value: "War Domain", img: "/images/subclasses/clericwar-subclass.png", desc: "Mixes martial prowess with divine wrath." },
        { value: "Knowledge Domain", img: "/images/subclasses/clericknowledge-subclass.png", desc: "Pursues divine insight and secrets." },
        { value: "Nature Domain", img: "/images/subclasses/clericnature-subclass.png", desc: "Protects flora, fauna, and the natural order." },
        { value: "Tempest Domain", img: "/images/subclasses/clerictempest-subclass.png", desc: "Wields lightning and thunder in divine wrath." },
        { value: "Trickery Domain", img: "/images/subclasses/clerictrickster-subclass.png", desc: "Employs deception as a holy tool." }
    ],
    druid: [
        { value: "Circle of the Land", img: "/images/subclasses/circleofland-subclass.png", desc: "Draws arcane power from a chosen land type." },
        { value: "Circle of the Moon", img: "/images/subclasses/circleofmoon-subclass.png", desc: "Masters combat Wild Shape with powerful beast forms." },
        { value: "Circle of Spores", img: "/images/subclasses/circleofspore-subclass.png", desc: "Harnesses fungal growth for necrotic power." },
        { value: "Circle of Stars", img: "/images/subclasses/circleofstars-subclass.png", desc: "Channels cosmic constellations and star maps." }
    ],
    fighter: [
        { value: "Champion", img: "/images/subclasses/champion-subclass.png", desc: "Focuses on raw physical prowess and wider crit range." },
        { value: "Battle Master", img: "/images/subclasses/battlemaster-subclass.png", desc: "Adopts maneuvers for battlefield control." },
        { value: "Eldritch Knight", img: "/images/subclasses/fightereldritchknight-subclass.png", desc: "Blends arcane magic with martial prowess." },
        { value: "Arcane Archer", img: "/images/subclasses/fighterarcanearcher-subclass.png", desc: "Infuses arrows with magical effects." }
    ],
    monk: [
        { value: "Way of the Open Hand", img: "/images/subclasses/monkopenhand-subclass.png", desc: "Masters of unarmed strikes and ki manipulation." },
        { value: "Way of Shadow", img: "/images/subclasses/monkshadow-subclass.png", desc: "Utilizes darkness for subterfuge and swift strikes." },
        { value: "Way of the Four Elements", img: "/images/subclasses/monkfourelemants-subclass.png", desc: "Channels elemental forces through martial arts." },
        { value: "Way of the Drunken Master", img: "/images/subclasses/monkdrunkmaster-subclass.png", desc: "Unpredictable movements and staggering steps." }
    ],
    paladin: [
        { value: "Oath of Devotion", img: "/images/subclasses/paladindevotion-subclass.png", desc: "Embodies honesty, purity, and chivalric virtue." },
        { value: "Oath of the Ancients", img: "/images/subclasses/paladinancients-subclass.png", desc: "Defends nature and fosters renewal and light." },
        { value: "Oath of Vengeance", img: "/images/subclasses/paladinvengence-subclass.png", desc: "Hunts down evildoers with merciless resolve." },
        { value: "Oathbreaker", img: "/images/subclasses/paladinoathbreaker-subclass.png", desc: "A fallen paladin using unholy powers." },
        { value: "Oath of the Crown", img: "/images/subclasses/paladincrown-subclass.png", desc: "Upholds civilization, law, and loyalty." }
    ],
    ranger: [
        { value: "Hunter", img: "/images/subclasses/rangerhunter-subclass.png", desc: "Chooses specialized tactics for monstrous foes." },
        { value: "Beast Master", img: "/images/subclasses/rangerbeastmaster-subclass.png", desc: "Forms a close bond with a loyal beast companion." },
        { value: "Gloom Stalker", img: "/images/subclasses/rangergloomstalker-subclass.png", desc: "Thrives in the dark, striking from the shadows." },
        { value: "Swarmkeeper", img: "/images/subclasses/rangerswarmkeeper-subclass.png", desc: "Manifests a writhing swarm of nature spirits." }
    ],
    rogue: [
        { value: "Thief", img: "/images/subclasses/roguethief-subclass.png", desc: "Experts in stealth, infiltration, and quick movement." },
        { value: "Assassin", img: "/images/subclasses/rogueassassin-subclass.png", desc: "Strikes from the shadows for devastating surprise attacks." },
        { value: "Arcane Trickster", img: "/images/subclasses/arcane-trickster-subclass.png", desc: "Merges roguish stealth with minor spellcasting." },
        { value: "Swashbuckler", img: "/images/subclasses/rogueswashbuckler-subclass.png", desc: "Charismatic duelists who thrive in single combat." },
        { value: "Soulknife", img: "/images/subclasses/soulknife-subclass.png", desc: "Psionic assassins who manifest psychic blades." }
    ],
    sorcerer: [
        { value: "Draconic Bloodline", img: "/images/subclasses/sorcererdragonic-subclass.png", desc: "Arcane power inherited from dragon ancestry." },
        { value: "Wild Magic", img: "/images/subclasses/sorcererwild-subclass.png", desc: "Chaotic surges that can reshape reality." },
        { value: "Shadow Magic", img: "/images/subclasses/sorcerershadow-subclass.png", desc: "Harnesses the power of darkness." },
        { value: "Storm Sorcery", img: "/images/subclasses/sorcererstorm-subclass.png", desc: "Channels the power of raging storms." }
    ],
    warlock: [
        { value: "The Fiend", img: "/images/subclasses/warlockfiend-subclass.png", desc: "Pact with a demon or devil for destructive flames." },
        { value: "The Archfey", img: "/images/subclasses/warlockarchfey-subclass.png", desc: "Gains whimsical and beguiling Feywild powers." },
        { value: "The Great Old One", img: "/images/subclasses/warlockgreatoldone-subclass.png", desc: "Taps into cosmic horrors for forbidden knowledge." },
        { value: "The Hexblade", img: "/images/subclasses/warlockhexblade-subclass.png", desc: "Channels the power of a sentient weapon." }
    ],
    wizard: [
        { value: "Abjuration", img: "/images/subclasses/wizardabjuration-subclass.png", desc: "Protective wards and anti-magic effects." },
        { value: "Conjuration", img: "/images/subclasses/wizardconjuration-subclass.png", desc: "Summons creatures and objects." },
        { value: "Divination", img: "/images/subclasses/wizarddivination-subclass.png", desc: "Foresight and portents to reshape fate." },
        { value: "Enchantment", img: "/images/subclasses/wizardenchantment-subclass.png", desc: "Controlling minds and emotions." },
        { value: "Evocation", img: "/images/subclasses/wizardevocation-subclass.png", desc: "Blasts foes with potent elemental spells." },
        { value: "Illusion", img: "/images/subclasses/wizardillusion-subclass.png", desc: "Weaves deceptive imagery and phantasms." },
        { value: "Necromancy", img: "/images/subclasses/wizardnecromancy-subclass.png", desc: "Manipulates life energy, animates undead." },
        { value: "Transmutation", img: "/images/subclasses/wizardtransmutation-subclass.png", desc: "Alters matter and energy." },
        { value: "Bladesinging", img: "/images/subclasses/wizardbladesinging-subclass.png", desc: "Melds swordplay with arcane dance." }
    ]
};

// 2) Render function: inject grid cards
function renderSubclassOptions(chosenClass) {
    // hide all subtype groups
    document.querySelectorAll('.subclass-group').forEach(div => {
        div.style.display = 'none';
        div.innerHTML = '';
    });

    const group = document.querySelector(`.subclass-group[data-parent="${chosenClass}"]`);
    if (!group) return;

    const level = parseInt(document.getElementById('characterLevel').value, 10);
    const req = parseInt(group.dataset.requiredLevel, 10);
    const container = document.getElementById('subclass-container');
    if (level < req) {
        container.style.display = 'none';
        return;
    }

    container.style.display = 'block';
    group.classList.add('grid');
    group.style.display = 'grid';

    window._subclassOptions[chosenClass].forEach(opt => {
        const label = document.createElement('label');
        label.className = 'option-card';
        label.innerHTML = `
      <input type="radio" name="subclass" value="${opt.value}">
      <img src="${opt.img}" alt="${opt.value}">
      <h4>${opt.value}</h4>
      <p><strong>Description:</strong> ${opt.desc}</p>
    `;
        group.appendChild(label);
    });
}

// 3) Wire up events when DOM ready
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('input[name="class"]').forEach(radio => {
        radio.addEventListener('change', e => renderSubclassOptions(e.target.value.toLowerCase()));
    });

    document.getElementById('characterLevel')
        .addEventListener('input', () => {
            const checked = document.querySelector('input[name="class"]:checked');
            if (checked) checked.dispatchEvent(new Event('change'));
        });
});
