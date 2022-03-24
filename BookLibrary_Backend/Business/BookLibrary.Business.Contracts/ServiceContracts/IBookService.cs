﻿using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using BookLibrary.Business.Contracts.DataContracts;

namespace BookLibrary.Business.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IBookService
    {
        /// <summary>
        /// RESTful command: GET.
        /// Response Resource: Json of LibraryBookData Array.
        /// Description: Query database to get 'x' items Library books information of page 'n' with borrowing availability status.
        /// </summary>
        /// <param name="page">page number between 1 to n.</param>
        /// <param name="item">items per page. can only one of: 10, 20, 30, 40, 50. (default: 10)</param>
        /// <returns>a Json format of LibraryBookData array.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "books?page={page}&item={item}", 
            ResponseFormat = WebMessageFormat.Json, 
            RequestFormat=WebMessageFormat.Json)]
        LibraryBookData[] GetBooks(int page, int item);
    }
}
