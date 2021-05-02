using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{

    public static AudioClip MusicMenu, MusicGame, BallHitGroundSound, BallHitBallSound, ScoreSound, ReadySound, FlyBallSound;
    static AudioSource audioSrc;

    void Start()
    {
        MusicMenu = Resources.Load<AudioClip>("Main");
        MusicGame = Resources.Load<AudioClip>("Game");
        BallHitGroundSound = Resources.Load<AudioClip>("Ground");
        BallHitBallSound = Resources.Load<AudioClip>("Balls");
        ScoreSound = Resources.Load<AudioClip>("Score");
        ReadySound = Resources.Load<AudioClip>("Ready");
        FlyBallSound = Resources.Load<AudioClip>("Fly");


        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        switch (clip)

        {
            case "Main":
                audioSrc.PlayOneShot(MusicMenu);
                break;
            case "Game":
                audioSrc.PlayOneShot(MusicGame);
                break;
            case "Ground":
                audioSrc.PlayOneShot(BallHitGroundSound);
                break;
            case "Balls":
                audioSrc.PlayOneShot(BallHitBallSound);
                break;
            case "Score":
                audioSrc.PlayOneShot(ScoreSound);
                break;
            case "Ready":
                audioSrc.PlayOneShot(ReadySound);
                break;
            case "Fly":
                audioSrc.PlayOneShot(FlyBallSound);
                break;

        }
    }
}
