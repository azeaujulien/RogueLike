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

    /// <summary>
    ///     This function init all UI info
    /// </summary>
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

    /// <summary>
    ///     This function change the current spell and update UI
    /// </summary>
    /// <param name="newSpell">New spell who replace the current</param>
    public void ChangeSpell(Spell newSpell)
    {
        currentSpell = newSpell;
        InitGraphics();
    }

    /// <summary>
    ///     This function close the spell's menu
    /// </summary>
    public void CloseMenu()
    {
        spellsMenu.SetActive(false);
    }
}
