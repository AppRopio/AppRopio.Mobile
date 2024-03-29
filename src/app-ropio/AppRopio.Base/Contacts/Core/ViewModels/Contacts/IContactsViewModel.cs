﻿using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Contacts.Responses;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Contacts.Core.ViewModels.Contacts
{
    public interface IContactsViewModel : IBaseViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        List<ListResponseItem> Contacts { get; }
    }
}