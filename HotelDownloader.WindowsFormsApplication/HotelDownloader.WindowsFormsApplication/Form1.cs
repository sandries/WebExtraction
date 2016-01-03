namespace HotelDownloader.WindowsFormsApplication
{
	using System;
	using System.Text;
	using System.Windows.Forms;

	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
			this.loadInitialHotel();
		}

		private void loadInitialHotel()
		{
			textBox1.Text =
				@"http://www.booking.com/hotel/de/adlon-kempinski-berlin.en-gb.html?label=gen173nr-15CAEoggJCAlhYSDNiBW5vcmVmaDuIAQGYAS64AQTIAQTYAQPoAQE;sid=cf06cf25ab793e3929e2ad223fe7c6d6;dcid=1;dist=0;group_adults=2;room1=A%2CA;sb_price_type=total;srfid=901b00f6735ad0d4bc32398cd94f6a4c84250a63X1;type=total;ucfs=1&";
			webBrowser.Url = new Uri(textBox1.Text);
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				webBrowser.Url = new Uri(textBox1.Text);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var hotel = (dynamic)HotelDownloader.GetHotel(webBrowser.DocumentText);
			textBox2.Text = getHotelDetails(hotel);
		}

		private string getHotelDetails(dynamic hotel)
		{
			StringBuilder hotelDetails = new StringBuilder();
			hotelDetails.AppendLine("Name: " + hotel.name + Environment.NewLine);
			hotelDetails.AppendLine("Address: " + hotel.address + Environment.NewLine);
			hotelDetails.AppendLine("Classification: " + hotel.classification + Environment.NewLine);
			hotelDetails.AppendLine("Review score: " + hotel.reviewscore + Environment.NewLine);
			hotelDetails.AppendLine("Number of reviews: " + hotel.numberofreviews + Environment.NewLine);


			hotelDetails.AppendLine("Room categories: ");
			foreach (var room in hotel.rooms)
			{
				hotelDetails.AppendLine(" - " + room);
			}
			hotelDetails.AppendLine();

			hotelDetails.AppendLine("Alternative hotels: ");
			foreach (var alternativeHotel in hotel.alternativeHotels)
			{
				hotelDetails.AppendLine(" - " + alternativeHotel);
			}
			hotelDetails.AppendLine();

			hotelDetails.AppendLine("Description: ");

			hotelDetails.AppendLine(hotel.description);
			return hotelDetails.ToString();
		}
	}
}
