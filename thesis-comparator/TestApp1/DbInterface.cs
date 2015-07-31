using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.IO;
using MongoDB.Driver.Builders;

namespace TestApp1
{
    class DbInterface
    {
        private String host;
        private String port;
        private String username;
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase docsRaw;
        private MongoCollection docsColl;
        
        public DbInterface(String host)
        {
            this.host = host;
            this.client = new MongoClient("mongodb://"+host);
            this.server = client.GetServer();
            this.docsRaw = server.GetDatabase("docs_raw");
            this.docsColl = docsRaw.GetCollection<BsonDocument>("fs.files");
        }

        public List<String> GetDocumentIds()
        {
            List<String> ids = new List<String>();
            foreach (var doc in docsColl.FindAs<BsonDocument>(Query.NE("id", "asdf")))
            {
                
                ids.Add(doc["fileId"].ToString());
            }

            return ids;
        }

        public String GetDocument(String id)
        {
            var doc = docsRaw.GridFS.FindOne(Query.EQ("fileId", id));
            Console.Out.WriteLine("Getting " + doc.Name + " " + doc.Id );
            StreamReader reader = new StreamReader(doc.OpenRead());
            var s = reader.ReadToEnd();
            //Console.Out.Write(s);
            return s;
            
        }

        internal string GetDocumentName(string id)
        {
            var doc = docsColl.FindAs<BsonDocument>(Query.EQ("fileId", id)).First();
            if (doc["orginalUrl"] != "")
            {
                return doc["orginalUrl"].AsString;
            }
            return doc["filename"].AsString;
            
        }

        public String GetDocuments()
        {
            foreach ( var doc in docsRaw.GridFS.FindAll()){
                StreamReader reader = new StreamReader(doc.OpenRead());
                Console.Out.WriteLine(reader.ReadToEnd());
                Console.ReadKey();
            }
            return "test";
        }



       
    }

    class DbDoc
    {
        public string A { get; set; }
        public string B { get; set; }
    } 
}
