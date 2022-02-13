namespace Model.Settings;

public class ClientProperties
{
    public ClientProperties()
    {
    }

    public ClientProperties(string baseClientUrl)
    {
        BaseClientUrl = baseClientUrl;
    }

    public string BaseClientUrl { get; set; }
}