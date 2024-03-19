using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TopDownDungeon.Audio;
internal class AudioController
{
    private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    internal void PlayAtBoundaryEffect()
    {
        using (var player = new SoundPlayer())
        {
            player.SoundLocation = _baseDirectory + "Assets\\Thump.wav";
            player.Play();
        }
    }
    internal void PlayEncounterEffect()
    {
        using (var player = new SoundPlayer())
        {
            player.SoundLocation = _baseDirectory + "Assets\\Growl.wav";
            player.Play();
        }
    }
    internal void PlayWalkingEffect()
    {
        using (var player = new SoundPlayer())
        {
            player.SoundLocation = _baseDirectory + "Assets\\Short Footsteps.wav";
            player.Play();
        }
    }
    internal void PlayWinnerEffect()
    {
        using (var player = new SoundPlayer())
        {
            player.SoundLocation = _baseDirectory + "Assets\\Win Tone.wav";
            player.Play();
        }
    }
}
