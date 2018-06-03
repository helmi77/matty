namespace Macli.Synapse.DTO
{
    class Credentials
    {
        public string user { get; set; }
        public string password { get; set; }
        public string type => "m.login.password";
    }
}
