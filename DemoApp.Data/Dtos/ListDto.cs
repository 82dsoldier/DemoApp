namespace DemoApp.Data.Dtos {
	/// <summary>
	/// The ListDto class provides a generic class that any entity can use to return data properly formatted
	/// for inclusion in an HTML select tag.
	/// </summary>
	public class ListDto {
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
