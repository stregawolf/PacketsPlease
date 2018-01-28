using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/CharacterData", order = 1)]
public class CharacterData : ScriptableObject {
    public CustomerData.Race m_race;

    public Sprite[] m_bodies;
    public Sprite[] m_heads;
    public Sprite[] m_neutralFaces;
    public Sprite[] m_positiveFaces;
    public Sprite[] m_negativeFaces;
    public Sprite[] m_hairs;

    public void GenerateCharacter(int seed, Image body, Image head, Image face, Image hair)
    {
        body.enabled = m_bodies.Length > 0;
        head.enabled = m_heads.Length > 0;
        face.enabled = m_neutralFaces.Length > 0;
        hair.enabled = m_hairs.Length > 0;

        body.sprite = GetSprite(m_bodies, seed);
        head.sprite = GetSprite(m_heads, seed);
        face.sprite = GetSprite(m_neutralFaces, seed);
        hair.sprite = GetSprite(m_hairs, seed);
    }

    public Sprite GetSprite(Sprite[] spriteSet, int seed)
    {
        if(spriteSet.Length > 0)
        {
            Random.InitState(seed);
            return spriteSet[Random.Range(0, spriteSet.Length)];
        }
        return null;
    }
}
