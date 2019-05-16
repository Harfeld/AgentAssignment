using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;


namespace opgave1
{
    public class Agent
    {
        public string id;
        public string codeName;
        public string specialty;
        public string assignment;
        public Agent()
        {}

        public Agent(string _id, string _codename, string _specialty, string _assignment)
        {
            id = _id;
            codeName = _codename;
            specialty = _specialty;
            assignment = _assignment;
        }

        public string ID { get { return id; } set { id = value; } }

        public string CodeName { get { return codeName; } set { codeName = value; } }

        public string Specialty { get { return specialty; } set { specialty = value; } }

        public string Assignment { get { return assignment; } set { assignment = value; } }
    }
}
