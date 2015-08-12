using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.IO;

namespace Riot_API_1
{
    public partial class Form1 : Form
    {
        private RiotApi riot;
        private List<string> matchidList; 

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            riot = new RiotApi();
            
            var client = new MongoClient();

            var db = client.GetDatabase("lol");

            var collection = db.GetCollection<BsonDocument>("butchers");

            // Alter this for generic
            foreach (var json in matchidList.Select(match => riot.GetDataFromUrl(riot.MatchStringBuilder("https://euw.api.pvp.net/api/lol/euw/v2.2", match, true))))
            {
                using (var jsonReader = new JsonReader(json))
                {
                    var doc = BsonSerializer.Deserialize<BsonDocument>(json);
                    await AddMongo(doc, collection);
                }
            }
        }

        private static async Task AddMongo(BsonDocument doc, IMongoCollection<BsonDocument> collection)
        {
            await collection.InsertOneAsync(doc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*var client = new MongoClient();

            var db = client.GetDatabase("lol");

            var collection = db.CreateCollectionAsync("butchers");*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(); // Show the dialog.

            if (result != DialogResult.OK) return;

            var file = openFileDialog1.FileName;

            try
            {
                //string text = File.ReadAllText(file);
                var text = File.ReadAllLines(file);

                matchidList = new List<string>(text.ToList());

                matchidList.Remove("[");
                matchidList.Remove("]");
                matchidList.Remove("\n\r");

                for (var i = 0; i < matchidList.Count; i++)
                {
                    if (matchidList[i].Contains(", "))
                        matchidList[i]= matchidList[i].Replace(", ", "");
                }

                MessageBox.Show("Succefully loaded " + matchidList.Count + " id's");
            }
            catch (IOException)
            {
                MessageBox.Show("Some error happened!");
            }
        }
    }
}
