using System.Collections.Generic;
using UnityEngine;

public class SpellsMenu : MonoBehaviour
{
    public PlayerSpells playerSpells;
    public Spell[] spells;
    public RingCakePiece ringCakePiecePrefab;
    public float gapWidthDegree = 1f;
    private RingCakePiece[] _pieces;
    public SpellInfo spellInfo;

    private void Start()
    {
        float stepLength = 360f / spells.Length;
        float iconDist = Vector3.Distance(ringCakePiecePrefab.icon.transform.position, ringCakePiecePrefab.cakePiece.transform.position);
        
        _pieces = new RingCakePiece[spells.Length];

        for (int i = 0; i < spells.Length; i++) {
            _pieces[i] = Instantiate(ringCakePiecePrefab, transform);
            //set root element
            _pieces[i].transform.localPosition = Vector3.zero;
            _pieces[i].transform.localRotation = Quaternion.identity;

            //set cake piece
            _pieces[i].cakePiece.fillAmount = 1f / spells.Length - gapWidthDegree / 360f;
            _pieces[i].cakePiece.transform.localPosition = Vector3.zero;
            _pieces[i].cakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + gapWidthDegree / 2f + i * stepLength);
            _pieces[i].cakePiece.color = new Color(1f, 1f, 1f, 0.5f);

            //set icon
            _pieces[i].icon.transform.localPosition = _pieces[i].cakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            _pieces[i].icon.sprite = spells[i].spellImage;
            _pieces[i].icon.name = spells[i].spellName;
        }
    }
    
    private void Update()
    {
        float stepLength = 360f / spells.Length;
        float mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f);
        int activeElement = (int)(mouseAngle / stepLength);
        int activeGraphElement = activeElement + 1 >= spells.Length ? 0 : activeElement + 1;

        spellInfo.InitGraphics(spells[activeElement]);
        for (int i = 0; i < _pieces.Length; i++) {
            if (i == activeGraphElement) {
                _pieces[i].cakePiece.color = new Color(1f, 1f, 1f, .75f);
            } else {
                _pieces[i].cakePiece.color = new Color(1f, 1f, 1f, .5f);
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            playerSpells.ChangeSpell(spells[activeElement]);
            playerSpells.CloseMenu();
        }
    }

    private float NormalizeAngle(float a)
    {
        return (a + 360f) % 360f;
    }
}
