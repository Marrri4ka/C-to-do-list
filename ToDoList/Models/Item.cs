using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
public class Item   // class
{
private string _description;
private DateTime _duedate;     // field
private int _id;
// private int _categoryId;
// private static List<Item> _instances = new List<Item>{}; //list

public Item (string description, DateTime duedate, int id=0)     // constructor
{
	_description = description;
	_duedate = duedate;
	// _categoryId = categoryId;
	// _instances.Add(this); //what is this
	_id = id;
}

// public int GetCategoryId()
// {
//      return _categoryId;
// }


public string GetDescription()
{
	return _description;
}

public void SetDescription(string newDescription)
{
	_description = newDescription;
}

public DateTime GetDueDate()
{
	return _duedate;
}

public void SetDueDate(DateTime newDueDate)
{
	_duedate = newDueDate;
}







public static List<Item> GetAll()
{
	List<Item> allItems = new List<Item> {
	};

	MySqlConnection conn = DB.Connection();
	conn.Open();
	MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"SELECT * FROM items;";
	MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

	while (rdr.Read())
	{
		int itemId = rdr.GetInt32(0);
		string itemDescription = rdr.GetString(1);
		DateTime itemDueDate = rdr.GetDateTime(2);
		// int categoryId = rdr.GetInt32(3);
		Item newItem = new Item (itemDescription,itemDueDate, itemId);
		allItems.Add(newItem);
	}

	conn.Close();

	if (conn != null)
	{
		conn.Dispose();
	}

	return allItems;

}

public static List<Item> Sort()
{
	List<Item> allItems = new List<Item> {
	};

	MySqlConnection conn = DB.Connection();
	conn.Open();
	MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"SELECT * FROM items ORDER BY duedate DESC;";
	MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

	while (rdr.Read())
	{
		int itemId = rdr.GetInt32(0);
		string itemDescription = rdr.GetString(1);
		DateTime itemDueDate = rdr.GetDateTime(2);
		// int itemCategoryId = rdr.GetInt32(3);
		Item newItem = new Item (itemDescription,itemDueDate,itemId);
		allItems.Add(newItem);
	}

	conn.Close();

	if (conn != null)
	{
		conn.Dispose();
	}

	return allItems;

}

public void Delete()
{
	MySqlConnection conn = DB.Connection();
	conn.Open();
	var cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"DELETE FROM items WHERE id=@item_id;";
	MySqlParameter itemId = new MySqlParameter();
	itemId.ParameterName = "@item_id";
	itemId.Value = this._id;
	cmd.Parameters.Add(itemId);
	cmd.ExecuteNonQuery();
	conn.Close();
	if (conn != null)
	{
		conn.Dispose();
	}
}

public static void ClearAll()
{
	MySqlConnection conn = DB.Connection();
	conn.Open();
	var cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"DELETE FROM items;";
	cmd.ExecuteNonQuery();

	conn.Close();
	if(conn != null)
	{
		conn.Dispose();
	}
}
public override bool Equals(System.Object otherItem)
{
	if (!(otherItem is Item))
	{
		return false;
	}
	else
	{
		Item newItem = (Item) otherItem;

		bool idEquality = this.GetId() == newItem.GetId();
		bool descriptionEquality = this.GetDescription() == newItem.GetDescription();
		// bool categoryEquality = this.GetCategoryId() == newItem.GetCategoryId();
		return (idEquality && descriptionEquality);
	}
}

public void Save()
{
	MySqlConnection conn = DB.Connection();
	conn.Open();
	var cmd = conn.CreateCommand() as MySqlCommand;

	cmd.CommandText = @"INSERT INTO items (description, duedate) VALUES (@ItemDescription, @ItemDueDate);";

	MySqlParameter descriptionParameter = new MySqlParameter();
	descriptionParameter.ParameterName = "@ItemDescription";
	descriptionParameter.Value = this._description;
	cmd.Parameters.Add(descriptionParameter);

	MySqlParameter dueDateParameter = new MySqlParameter();
	dueDateParameter.ParameterName = "@ItemDueDate";
	dueDateParameter.Value = this._duedate;
	cmd.Parameters.Add(dueDateParameter);

	// MySqlParameter categoryParameter = new MySqlParameter();
	// categoryParameter.ParameterName = "@ItemCategoryId";
	// categoryParameter.Value = this._categoryId;
	// cmd.Parameters.Add(categoryParameter);

	cmd.ExecuteNonQuery();

	_id = (int) cmd.LastInsertedId;

	conn.Close();
	if (conn != null)
	{
		conn.Dispose();
	}
}


public void Edit(string newDescription, DateTime newduedate)
{
	MySqlConnection conn = DB.Connection();
	conn.Open();
	var cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";
	cmd.CommandText = @"UPDATE items SET duedate = @newduedate WHERE id = @searchId;";
	MySqlParameter searchId = new MySqlParameter();
	searchId.ParameterName = "@searchId";
	searchId.Value = _id;
	cmd.Parameters.Add(searchId);

	MySqlParameter description = new MySqlParameter();
	description.ParameterName = "@newDescription";
	description.Value = newDescription;
	cmd.Parameters.Add(description);
	MySqlParameter duedate = new MySqlParameter();
	duedate.ParameterName = "@newduedate";
	duedate.Value = newduedate;
	cmd.Parameters.Add(duedate);
	cmd.ExecuteNonQuery();

	_description = newDescription;
	_duedate = newduedate;

	conn.Close();
	if (conn != null)
	{
		conn.Dispose();
	}
}




public int GetId()
{
	return _id;
}
//
public static Item Find (int id)
{

	MySqlConnection conn = DB.Connection();
	conn.Open();
	var cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"SELECT * FROM items WHERE id = (@searchId);";
	MySqlParameter idParameter = new MySqlParameter();
	idParameter.ParameterName = "@searchId";
	idParameter.Value = id;
	cmd.Parameters.Add(idParameter);
	var rdr = cmd.ExecuteReader() as MySqlDataReader;
	int itemId=0;
	string itemDescription ="";
	DateTime itemDueDate = new DateTime();
	// int itemCategoryId = 0;

	while(rdr.Read())
	{
		itemId = rdr.GetInt32(0);
		itemDescription = rdr.GetString(1);
		itemDueDate = rdr.GetDateTime(2);
		// itemCategoryId = rdr.GetInt32(3);
	}
	Item fountItem = new Item (itemDescription,itemDueDate, itemId);

	conn.Close();
	if(conn != null)
	{
		conn.Dispose();
	}
	return fountItem;
}


}
}
