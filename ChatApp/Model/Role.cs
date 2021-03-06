﻿namespace ChatApp.Model
{
    public class Role : Entity
    {
        private string name;

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged();}
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
