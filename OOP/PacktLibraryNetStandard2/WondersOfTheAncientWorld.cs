namespace Packt.Shared;

/* Normally, an enum type uses an int variable internally. 
 * To reduce memory requirements when values that big aren't needed, derive an enum type from byte if there are up to eight options, 
 * from ushort if there are up to 16 options, from uint if there are up to 32 options, and from ulong if there are up to 64 options. */

[Flags]
public enum WondersOfTheAncientWorld : byte
{
    None                        = 0b_0000_0000, // i.e. 0
    GreatPyramidOfGiza          = 0b_0000_0001, // i.e. 1
    HangingGardensOfBabylon     = 0b_0000_0010, // i.e. 2
    StatueOfZeusAtOlympia       = 0b_0000_0100, // i.e. 4
    TempleOfArtemisAtEphesus    = 0b_0000_1000, // i.e. 8
    MausoleumAtHalicarnassus    = 0b_0001_0000, // i.e. 16
    ColossusOfRhodes            = 0b_0010_0000, // i.e. 32
    LighthouseOfAlexandria      = 0b_0100_0000  // i.e. 64
}
