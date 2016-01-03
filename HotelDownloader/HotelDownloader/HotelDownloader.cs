namespace HotelDownloader
{
	using System;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Web;
	using HtmlAgilityPack;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public static class HotelDownloader
	{
		public static string GetJson(string text)
		{
			var hotel = GetHotel(text);
			string json = JsonConvert.SerializeObject(hotel);
			return json;
		}

		private static JObject GetHotel(string text)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(text);

			dynamic hotel = new JObject();

			hotel.name = doc.DocumentNode.SelectSingleNode("//span[@id='hp_hotel_name']").InnerText.Decode();
			hotel.address = doc.DocumentNode.SelectSingleNode("//span[@id='hp_address_subtitle']").InnerText.Decode();
			hotel.rooms = new JArray(doc.DocumentNode.SelectNodes("//td[@class='ftd']").Select(n => n.InnerText.Decode()));
			hotel.reviewscore = doc.DocumentNode.SelectSingleNode("//span[@class='average js--hp-scorecard-scoreval']").InnerText.Decode();
			hotel.numberofreviews = doc.DocumentNode.SelectSingleNode("//span[@class='trackit score_from_number_of_reviews']").ChildNodes[1].InnerText.Decode();
			hotel.alternativeHotels =
				new JArray(
					doc.DocumentNode.SelectSingleNode("//table[@id='althotelsTable']")
						.SelectNodes("//a[@class='althotel_link']")
						.Select(n => n.InnerText.Decode()));
			hotel.description =
				new JArray(
					doc.DocumentNode.SelectSingleNode("//div[@id='summary']")
						.ChildNodes.Where(n => n.Name == "p")
						.Select(n => n.InnerText.Decode())
						.Aggregate((x, y) => x + Environment.NewLine + Environment.NewLine + y));

			var hotelRatingNode = doc.DocumentNode.SelectSingleNode("//span[@class='hp__hotel_ratings__stars hp__hotel_ratings__stars__clarification_track']");
			if (hotelRatingNode.HasChildNodes)
			{
				var child = hotelRatingNode.ChildNodes.FirstOrDefault(n => n.Name == "i");
				if (child != null)
				{
					var regex = new Regex("[a-w_]*[1-9]");
					var result = regex.Match(child.Attributes["class"].Value.Decode());
					if (result.Success)
					{
						hotel.classification = result.Value;
					}
				}
			}
			return hotel;
		}
	}

	internal static class StringExtensions
	{
		public static string Decode(this string value)
		{
			if (value != null)
			{
				return HttpUtility.HtmlDecode(value).Replace(Environment.NewLine, " ").Trim();
			}
			return value;
		}
	}
}