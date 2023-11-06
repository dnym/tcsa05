using Flashcards.DataAccess;
using Flashcards.DataAccess.MockDB;
using Flashcards.Models;
using TCSAHelper.General;

namespace Flashcards;

internal static class Program
{
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    static void Main()
    {
        IDataAccess _dataAccess = new MockDB();
        AddDummyData(_dataAccess);
        var screen = UI.MainMenu.Get(_dataAccess);
        screen.Show();
        Console.Clear();
    }

    private static void AddDummyData(IDataAccess dataAccess)
    {
        // A few examples from https://en.wiktionary.org/wiki/Appendix:Swadesh_lists
        Dictionary<string, List<Tuple<string, string>>> data = new()
        {
            {
                "Bulgarian", new()
                {
                    new Tuple<string, string>("sun", "slǎnce)"),
                    new Tuple<string, string>("moon", "luna"),
                    new Tuple<string, string>("star", "zvezda"),
                    new Tuple<string, string>("water", "voda"),
                    new Tuple<string, string>("rain", "dǎžd"),
                    new Tuple<string, string>("river", "reka"),
                    new Tuple<string, string>("lake", "ezero"),
                    new Tuple<string, string>("sea", "more"),
                    new Tuple<string, string>("salt", "sol"),
                    new Tuple<string, string>("stone", "kamǎk"),
                    new Tuple<string, string>("sand", "pjasǎk"),
                    new Tuple<string, string>("dust", "prah"),
                    new Tuple<string, string>("earth", "zemja"),
                    new Tuple<string, string>("cloud", "oblak"),
                    new Tuple<string, string>("fog", "mǎgla"),
                    new Tuple<string, string>("sky", "nebe"),
                    new Tuple<string, string>("wind", "vjatǎr"),
                    new Tuple<string, string>("snow", "snjag"),
                    new Tuple<string, string>("ice", "led"),
                    new Tuple<string, string>("smoke", "pušek"),
                    new Tuple<string, string>("fire", "ogǎn"),
                    new Tuple<string, string>("ash", "pepel"),
                }
            },
            {
                "Finnish", new()
                {
                    new Tuple<string, string>("to drink", "juoda"),
                    new Tuple<string, string>("to eat", "syödä"),
                    new Tuple<string, string>("to bite", "purra"),
                    new Tuple<string, string>("to suck", "imeä"),
                    new Tuple<string, string>("to spit", "sylkeä"),
                    new Tuple<string, string>("to vomit", "oksentaa"),
                    new Tuple<string, string>("to blow", "puhaltaa"),
                    new Tuple<string, string>("to breathe", "hengittää"),
                    new Tuple<string, string>("to laugh", "nauraa"),
                    new Tuple<string, string>("to see", "nähdä"),
                    new Tuple<string, string>("to hear", "kuulla"),
                    new Tuple<string, string>("to know", "tietää"),
                    new Tuple<string, string>("to think", "ajatella"),
                    new Tuple<string, string>("to smell", "haistaa"),
                    new Tuple<string, string>("to fear", "pelätä"),
                    new Tuple<string, string>("to sleep", "nukkua"),
                    new Tuple<string, string>("to live", "elää"),
                    new Tuple<string, string>("to die", "kuolla"),
                    new Tuple<string, string>("to kill", "tappaa")
                }
            },
            {
                "German", new()
                {
                    new Tuple<string, string>("mother", "Mutter"),
                    new Tuple<string, string>("father", "Vater"),
                    new Tuple<string, string>("animal", "Tier"),
                    new Tuple<string, string>("fish", "Fisch"),
                    new Tuple<string, string>("bird", "Vogel"),
                    new Tuple<string, string>("dog", "Hund"),
                    new Tuple<string, string>("louse", "Laus"),
                    new Tuple<string, string>("snake", "Schlange"),
                    new Tuple<string, string>("worm", "Wurm"),
                    new Tuple<string, string>("tree", "Baum"),
                    new Tuple<string, string>("forest", "Forst, Wald"),
                    new Tuple<string, string>("stick", "Stock"),
                    new Tuple<string, string>("fruit", "Frucht, Obst"),
                    new Tuple<string, string>("seed", "Samen, Saat"),
                    new Tuple<string, string>("leaf", "Blatt"),
                    new Tuple<string, string>("root", "Wurzel")
                }
            },
            {
                "Greek", new()
                {
                    new Tuple<string, string>("day", "méra"),
                    new Tuple<string, string>("year", "chrónos"),
                    new Tuple<string, string>("warm", "zestós"),
                    new Tuple<string, string>("cold", "krýos"),
                    new Tuple<string, string>("full", "gemátos"),
                    new Tuple<string, string>("new", "kainoúrgios"),
                    new Tuple<string, string>("old", "paliós"),
                    new Tuple<string, string>("good", "kalós"),
                    new Tuple<string, string>("bad", "kakós"),
                    new Tuple<string, string>("rotten", "sápios"),
                    new Tuple<string, string>("dirty", "vrómikos"),
                    new Tuple<string, string>("straight", "ísios"),
                    new Tuple<string, string>("round", "strongylós")
                }
            },
            {
                "Indonesian", new()
                {
                    new Tuple<string, string>("one", "satu"),
                    new Tuple<string, string>("two", "dua"),
                    new Tuple<string, string>("three", "tiga"),
                    new Tuple<string, string>("four", "empat"),
                    new Tuple<string, string>("five", "lima"),
                    new Tuple<string, string>("big", "besar"),
                    new Tuple<string, string>("long", "panjang"),
                    new Tuple<string, string>("wide", "lebar"),
                    new Tuple<string, string>("thick", "tebal"),
                    new Tuple<string, string>("heavy", "berat"),
                    new Tuple<string, string>("small", "kecil"),
                    new Tuple<string, string>("short", "pendek"),
                    new Tuple<string, string>("narrow", "sempit"),
                    new Tuple<string, string>("thin", "tipis"),
                    new Tuple<string, string>("woman", "perempuan"),
                    new Tuple<string, string>("man (adult male)", "lelaki")
                }
            },
            {
                "Japanese", new()
                {
                    new Tuple<string, string>("I ", "わたし"),
                    new Tuple<string, string>("you (singular)", "あなた"),
                    new Tuple<string, string>("he ", "かれ"),
                    new Tuple<string, string>("we ", "わたしたち"),
                    new Tuple<string, string>("you (plural)", "あなたたち"),
                    new Tuple<string, string>("they ", "かれら"),
                    new Tuple<string, string>("this ", "これ"),
                    new Tuple<string, string>("that ", "それ"),
                    new Tuple<string, string>("here ", "ここ"),
                    new Tuple<string, string>("there ", "そこ"),
                    new Tuple<string, string>("who ", "だれ"),
                    new Tuple<string, string>("what ", "なに"),
                    new Tuple<string, string>("where ", "どこ"),
                    new Tuple<string, string>("when ", "いつ"),
                    new Tuple<string, string>("how ", "いかが"),
                    new Tuple<string, string>("not ", "ない ")
                }
            },
            {
                "Latin", new()
                {
                    new Tuple<string, string>("breast", "mamma"),
                    new Tuple<string, string>("heart", "cor"),
                    new Tuple<string, string>("liver", "iecur"),
                    new Tuple<string, string>("to drink", "bibo"),
                    new Tuple<string, string>("to eat", "edo"),
                    new Tuple<string, string>("to bite", "mordeo"),
                    new Tuple<string, string>("to suck", "sugo"),
                    new Tuple<string, string>("to spit", "spuo"),
                    new Tuple<string, string>("to vomit", "vomo"),
                    new Tuple<string, string>("to blow", "inflo"),
                    new Tuple<string, string>("to breathe", "respiro"),
                    new Tuple<string, string>("to laugh", "rideo"),
                    new Tuple<string, string>("to see", "video"),
                    new Tuple<string, string>("to hear", "audio"),
                    new Tuple<string, string>("to know", "scio ")
                }
            },
            {
                "Swedish", new()
                {
                    new Tuple<string, string>("egg", "ägg"),
                    new Tuple<string, string>("horn", "horn"),
                    new Tuple<string, string>("tail", "svans"),
                    new Tuple<string, string>("feather", "fjäder"),
                    new Tuple<string, string>("hair", "hår"),
                    new Tuple<string, string>("head", "huvud"),
                    new Tuple<string, string>("ear", "öra"),
                    new Tuple<string, string>("eye", "öga"),
                    new Tuple<string, string>("nose", "näsa"),
                    new Tuple<string, string>("mouth", "mun"),
                    new Tuple<string, string>("tooth", "tand"),
                    new Tuple<string, string>("tongue (organ)", "tunga"),
                    new Tuple<string, string>("fingernail", "nagel"),
                    new Tuple<string, string>("foot", "fot"),
                    new Tuple<string, string>("leg", "ben"),
                    new Tuple<string, string>("knee", "knä"),
                    new Tuple<string, string>("hand", "hand"),
                    new Tuple<string, string>("wing", "vinge"),
                    new Tuple<string, string>("belly", "mage"),
                    new Tuple<string, string>("guts", "inälvor"),
                    new Tuple<string, string>("neck", "hals"),
                    new Tuple<string, string>("back", "rygg"),
                    new Tuple<string, string>("breast", "bröst"),
                    new Tuple<string, string>("heart", "hjärta"),
                    new Tuple<string, string>("liver", "lever ")
                }
            }
        };
        foreach (var language in data.Keys)
        {
            var stack = new Stack(language);
            dataAccess.CreateStackAsync(new()
            {
                ViewName = stack.ViewName,
                SortName = stack.SortName
            }).Wait();
        }
        foreach (var stackListItem in dataAccess.GetStackListAsync().Result)
        {
            foreach (var (front, back) in data[stackListItem.ViewName])
            {
                dataAccess.CreateFlashcardAsync(new()
                {
                    StackId = stackListItem.Id,
                    Front = $"What's the {stackListItem.ViewName} word for: {front}?",
                    Back = back
                }).Wait();
            }
        }
    }
}
