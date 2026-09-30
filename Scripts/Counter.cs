using System.Collections.Generic;
using YG;

public static class Counter
{
    public static int _red = 0;
    public static int _green = 0;

    public static int red 
    {
        get { return _red;}
        set { _red = value; Metric(); }
    }

    

    public static int green
    {
        get { return _green; }
        set { _green = value; Metric(); }
    }


    static void Metric() 
    {
        switch (_red + _green) 
        {
            case 1: YandexMetrica.Send("triggers",new Dictionary<string, string> { {"Round","1" } });
                break;
            case 2: YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "2" } });
                break;
            case 5:
                YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "5" } });
                break;
            case 10:
                YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "10" } });
                break;
            case 20:
                YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "20" } });
                break;
            case 40:
                YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "40" } });
                break;
            case 70:
                YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "70" } });
                break;
            case 100:
                YandexMetrica.Send("triggers", new Dictionary<string, string> { { "Round", "100" } });
                break;
            default: break;
        }
    }

}
