﻿using System;

namespace ChatApp.Model
{
    public class UserInfo : Entity
    {
        private string username;
        private string email;
        private string firstName;
        private string lastName;
        private string company;
        private string country;
        private DateTime dateOfBirth;
        private string gender;

        public string Username
        {
            get => username;
            set { username = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }
        public string Company
        {
            get => company;
            set { company = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get => country;
            set { country = value; OnPropertyChanged(); }
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set { dateOfBirth = value; OnPropertyChanged(); }
        }

        public string Gender
        {
            get => gender;
            set { gender = value; OnPropertyChanged(); }
        }
    }
}
