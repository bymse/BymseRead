﻿using BymseBooks.DataLayer.Entity;

namespace BymseBooks.Core.Models;

public class BookModel
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string[] Tags { get; init; }
    public string Author { get; init; }
    public int? Percents { get; init; }
    public BookState State { get; init; }
    public int? TotalPages { get; set; }
}