using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] private GameObject spellsMenu;
    [SerializeField] private Image spellIcon;
    
    [HideInInspector] public Spell currentSpell;

    private void Start()
    {
        InitGraphics();
    }

    private void InitGraphics()
    {
        if (currentSpell != null) {
            spellIcon.sprite = currentSpell.spellImage;
            spellIcon.color = Color.white;
        } else {
            spellIcon.color = Color.clear;
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) {
            spellsMenu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && spellsMenu.activeSelf) {
            spellsMenu.SetActive(false);
        }
    }

    public void ChangeSpell(Spell newSpell)
    {
        currentSpell = newSpell;
        InitGraphics();
    }

    public void CloseMenu()
    {
        spellsMenu.SetActive(false);
    }
}
