using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using rent_calc.Logics;

namespace rent_calc.Logics
{
    [Serializable]
    public class Address : Object
    {
        public string name;
        public string description;
        public BindingList<Person> people;
        public string GetName()
        {
            return name;
        }
        public override string ToString()
        {
            return GetName();
        }
        public Address(string newName, string newDescription)
        {
            name = newName;
            description = newDescription;
            people = new BindingList<Person>();
        }
    }
}
