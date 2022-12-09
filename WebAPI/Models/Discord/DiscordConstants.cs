namespace WebAPI.Models.Discord
{
    public static class DiscordConstants
    {
        public static readonly IEnumerable<string> FangsRegionRoles = new[]
        {FangsRole_Europe, FangsRole_NorthAmerica, FangsRole_SouthAmerica, FangsRole_Asia, FangsRole_Oceania, FangsRole_Africa};
        public const string FangsRole_Europe = "807300064577323114";
        public const string FangsRole_NorthAmerica = "807299956326137876";
        public const string FangsRole_SouthAmerica = "807300007538982964";
        public const string FangsRole_Asia = "807308381177774100";
        public const string FangsRole_Oceania = "807310795532796004";
        public const string FangsRole_Africa = "842899953973329920";

        public const string FangsRole_Insider = "103419307603553484";
        public const string FangsRole_Moderator = "804466206299127818";
        public const string FangsRole_Developer = "775489503623118869";
        public const string FangsRole_ServerBooster = "784148581702434831";
        public const string FangsRole_Admin = "775488935500578817";

        public const string SuperId = "224586010271547393";

        public const string Claim_userId = "discordId";
        public const string Claim_userNick = "discordNick";
        public const string Claim_isdev = "discordDev";
        public const string Claim_ismod = "discordMod";
        public const string Claim_regionId = "regionRoleId";

    }
}
