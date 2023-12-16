using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PalestreProgetto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PalestreProgetto
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			MostraPalestre();
			MostraAttrezzi();
		}


		// AREA MOSTRA NELLE LISTBOX

		// Mostra Palestre nella Listbox "listPalestre"
		private void MostraPalestre()
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					string nomeTabella = "Palestra";
					var palestraList = context.Palestras.FromSqlRaw<Palestra>($"SELECT * FROM {nomeTabella}").ToList<Palestra>();
					listPalestre.ItemsSource = palestraList;
					listPalestre.DisplayMemberPath = "Location";
					listPalestre.SelectedValuePath = "Id";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Mostra Attrezzi nella Listbox "tuttiGliAttrezziList"
		private void MostraAttrezzi()
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					string nomeTabella = "Attrezzo";
					var listaCompleta = context.Attrezzos.FromSqlRaw<Attrezzo>($"Select attrezzo.* From {nomeTabella}").ToList<Attrezzo>();
					tuttiGliAttrezziList.ItemsSource = listaCompleta;
					tuttiGliAttrezziList.DisplayMemberPath = "Name";
					tuttiGliAttrezziList.SelectedValuePath = "Id";
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
		// Mostra attrezzi associati alla Palestra, nell'apposita listBox "attrezziList"
		private void MostraAttrezziAssociati(int palestraId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					string nomeTabellaAttrezzo = "Attrezzo";
					string nomeTabellaPalestraAttrezzo = "PalestraAttrezzo";
					string nomeTabellaPalestra = "Palestra";

					var attrezzisList = context.Attrezzos
				.FromSqlRaw<Attrezzo>($@"
                    SELECT a.*
                    FROM {nomeTabellaAttrezzo} a
                    INNER JOIN {nomeTabellaPalestraAttrezzo} pa ON a.Id = pa.Attrezzo_Id
                    INNER JOIN {nomeTabellaPalestra} p ON pa.Palestra_Id = p.Id
                    WHERE p.Id = @PalestraId", new SqlParameter("@PalestraId", palestraId))
				.ToList<Attrezzo>();

					// Imposta la nuova lista di attrezzi
					attrezziList.ItemsSource = attrezzisList;
					attrezziList.DisplayMemberPath = "Name";
					attrezziList.SelectedValuePath = "Id";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}



		// AREA SELECTION CHANGED
		private void listPalestre_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (listPalestre.SelectedValue != null)
			{
				int palestraId = (int)listPalestre.SelectedValue;

				// Passa l'ID della palestra al metodo MostraAttrezziAssociati
				MostraAttrezziAssociati(palestraId);
				// Method implementato nell'area USER INPUT, che mostra la location della palestra selezionata, nella textbox
				MostraPalestraInTextbox(palestraId);
			}
		}
		

		private void listAttrezzi_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (tuttiGliAttrezziList.SelectedValue != null)
			{
				int attrezzoId = (int)tuttiGliAttrezziList.SelectedValue;

				// Passa l'ID dell'attrezzo al metodo MostraAttrezzoInTextbox, che mostrerà il nome dell'attrezzo scelto nella TextBox
				MostraAttrezzoInTextbox(attrezzoId);
			}

		}

		// Metodo necessario al funzionamento del button "Cancella Palestra" (method CancellaPalestra_Click)
		private void CancellaPalestra_Selected(int palestraId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{

					var palestraCancella = context.Palestras.Find(palestraId);

					if (palestraCancella != null)
					{
						context.Palestras.Remove(palestraCancella);
						context.SaveChanges();
					}
					context.Dispose();
				}
			}
			catch (Exception e)
			{
				// MessageBox.Show(e.Message);
			}
		}
		// Metodo necessario al funzionamento del button "Cancella Attrezzo" (method CancellaAttrezzo_Click)
		private void CancellaAttrezzo_Selected(int attrezzoId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{

					var attrezzoCancella = context.Attrezzos.Find(attrezzoId);

					if (attrezzoCancella != null)
					{
						context.Attrezzos.Remove(attrezzoCancella);
						context.SaveChanges();
					}
					context.Dispose();
				}
			}
			catch (Exception e)
			{
				//MessageBox.Show(e.Message);
			}
		}


		// AREA BUTTON CANCELLA. (AREA COLLEGATA ALL'AREA SOPRASTANTE, SELECION CHANGED)

		// Button Cancella Palestra. Necessita del method CancellaPalestra_Selected, presente nell'area Selection Changed
		private void CancellaPalestra_Click(object sender, RoutedEventArgs e)
		{
			if (listPalestre.SelectedValue != null)
			{
				int palestraId = (int)listPalestre.SelectedValue;
				CancellaPalestra_Selected(palestraId);
			}
			// Aggiorna automaticamente il database dopo l'eliminazione
			UpdateDatabase();
			MostraPalestre();
		}

		// Button Cancella Attrezzo. Necessita del method CancellaAttrezzo_Selected, presente nell'area Selection Changed
		private void CancellaAttrezzo_Click(object sender, RoutedEventArgs e)
		{
			if (tuttiGliAttrezziList.SelectedValue != null)
			{
				int attrezziId = (int)tuttiGliAttrezziList.SelectedValue;
				CancellaAttrezzo_Selected(attrezziId);
			}
			// Aggiorna automaticamente il database dopo l'eliminazione
			MostraAttrezzi();
			UpdateDatabase();
		}




		// AREA BUTTON AGGIUNGI/RIMUOVI

		// Con questo button si Aggiunge una Palestra una volta scritta la location nella textbox "MyTextBox"
		// Necessita quindi del method AggiungiPalestra_Location presente nell'area User Input
		private void AggiungiPalestra_Click(object sender, RoutedEventArgs e)
		{
			AggiungiPalestra_Location(MyTextBox.Text);
			UpdateDatabase();
			MostraPalestre();
			MyTextBox.Clear();
			MyTextBox.Focus();
		}

		// Aggiungi attrezzo alla lista totale degli attrezzi, una volta scritto in nome nella textbox "MyTextBox"
		// Necessita quindi del method AggiungiAttrezzo_Nome presente nell'area User Input
		private void AggiungiAttrezzo_Click(object sender, RoutedEventArgs e)
		{
			AggiungiAttrezzo_Nome(MyTextBox.Text);
			UpdateDatabase();
			MostraAttrezzi();
			MyTextBox.Clear();
			MyTextBox.Focus();
		}

		// Permette di rimuovere un attrezzo (button Rimuovi Attrezzo),
		//  selezionato dalla listbox "attrezziList" dalla palestra selezionata dalla
		// listbox "listPalestre". Necessita del method, implementato subito dopo, RimuoviAttrezzoDaPalestra 
		private void RimuoviAttrezzoDaPalestra_Click(object sender, RoutedEventArgs e)
		{
			if (attrezziList.SelectedValue != null)
			{
				int attrezzoId = (int)attrezziList.SelectedValue;
				int palestraId = (int)listPalestre.SelectedValue;
				RimuoviAttrezzoDaPalestra(attrezzoId, palestraId);
				//MessageBox.Show($"Cancella attrezzo {attrezzoId} da palestra {palestraId}");
				MostraAttrezziAssociati(palestraId);
			}
			UpdateDatabase();
		}
		// Method che lavora al fine di consentire il funzionamento del button "Rimuovi Attrezzo"
		private void RimuoviAttrezzoDaPalestra(int attrezzoId, int palestraId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{

					var remove = context.PalestraAttrezzos.FirstOrDefault(za => za.AttrezzoId == attrezzoId && za.PalestraId == palestraId);
					if (remove != null)
					{

						// Rimuovi Attrezzo da PalestraAttrezzo e quindi tutto il record
						context.PalestraAttrezzos.Remove(remove);

						// Applica le modifiche nel database
						context.SaveChanges();
					}
					// Aggiorna la ListBox degli attrezzi
					MostraAttrezzi();
					context.Dispose();
				}
			}
			catch (Exception e)
			{
				//MessageBox.Show(e.Message);
			}
		}



		// Permette di aggiungere un attrezzo (button Aggiungi Alla Palestra),
		//  selezionato dalla listbox "tuttiGliAttrezziList" alla palestra selezionata dalla
		// listbox "listPalestre". Necessita del method, implementato subito dopo, AggiungiAttrezzo_aPalestra 
		private void AggiungiAttrezzoAllaPalestra_Click(object sender, RoutedEventArgs e)
		{
			if (listPalestre.SelectedValue != null && tuttiGliAttrezziList.SelectedValue != null)
			{
				int attrezzoId = (int)tuttiGliAttrezziList.SelectedValue;
				int palestraId = (int)listPalestre.SelectedValue;
				// Gli id per il method AggiungiAttrezzo_aPalestra sono forniti dai due SelectedValue
				AggiungiAttrezzo_aPalestra(palestraId, attrezzoId);
				MostraAttrezziAssociati(palestraId);
			}
			UpdateDatabase();
		}
		// Method che lavora al fine di consentire il funzionamento del button "Aggiungi Alla Palestra"
		private void AggiungiAttrezzo_aPalestra(int palestraId, int attrezzoId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					var palestra = context.Palestras.Find(palestraId);
					var attrezzo = context.Attrezzos.Find(attrezzoId);

					if (palestra != null && attrezzo != null)
					{
						// Crea un nuovo oggetto PalestraAttrezzo e imposta gli oggetti Palestra e Attrezzo
						var palestraAttrezzo = new PalestraAttrezzo()
						{
							Palestra = palestra,
							Attrezzo = attrezzo,
						};

						// Aggiungi l'oggetto PalestraAttrezzo al contesto
						context.PalestraAttrezzos.Add(palestraAttrezzo);

						// Applica le modifiche nel database
						context.SaveChanges();
					}
					// Aggiorna la ListBox degli attrezzi
					MostraAttrezzi();
					context.Dispose();
				}
			}
			catch (Exception e)
			{
				// MessageBox.Show(e.Message);
			}

		}



		// AREA BUTTON AGGIORNA


		// Button che aggiorna la palestra sulla base delle modifiche apportate dall'utente alla location nella textbox
		// necessita del method AggiornaPalestra, implementato subito dopo
		private void AggiornaPalestra_Click(object sender, RoutedEventArgs e)
		{
			if (listPalestre.SelectedValue != null)
			{
				int palestraId = (int)listPalestre.SelectedValue;
				AggiornaPalestra(palestraId);
			}
			UpdateDatabase();
		}
		// Metodo necessario a far funzionare il button "Aggiorna Palestra". Fa comparire la location della palestra nelle textbox
		private void AggiornaPalestra(int palestraId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					var palestra = context.Palestras.Find(palestraId);
					//  var location = MyTextBox.Text;

					if (palestra != null)
					{

						palestra.Location = MyTextBox.Text;

						context.SaveChanges();
					}
					// Aggiorna la ListBox delle Palestre
					MostraPalestre();
					context.Dispose();
				}
			}
			catch (Exception e)
			{
				// MessageBox.Show(e.Message);
			}

		}


		// Button che aggiorna l'attrezzo sulla base delle modifiche apportate dall'utente al nome nella textbox
		// necessita del method AggiornaAttrezzo, implementato subito dopo
		private void AggiornaAttrezzo_Click(object sender, RoutedEventArgs e)
		{
			if (tuttiGliAttrezziList.SelectedValue != null)
			{
				int attrezzoId = (int)tuttiGliAttrezziList.SelectedValue;
				AggiornaAttrezzo(attrezzoId);
			}
			UpdateDatabase();
		}
		// Metodo necessario a far funzionare il button "Aggiorna Attrezzo". Fa comparire la location della palestra nelle textbox
		private void AggiornaAttrezzo(int attrezzoId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					var attrezzo = context.Attrezzos.Find(attrezzoId);
					

					if (attrezzo != null)
					{

						attrezzo.Name = MyTextBox.Text;

						context.SaveChanges();
					}
					// Aggiorna la ListBox degli Attrezzi
					MostraAttrezzi();
					context.Dispose();
				}
			}
			catch (Exception e)
			{
				// MessageBox.Show(e.Message);
			}
		}


		// AREA USER INPUT IN TEXTBOX "MyTextBox" E MOSTRA NOME ATTREZZO O LOCATION PALESTRA IN "MyTextBox"

		// Questo method, inserito nel codice del button AggiungiPalestra_Click, agginge una nuova palestra, iserita la location
		// nella textbox da parte dell'utente
		private void AggiungiPalestra_Location(string text)
		{
			using (var context = new UdemyEseDbContext())
			{
				var palestra = new Palestra()
				{
					Location = text,
				};
				// Aggiungi l'oggetto Palestra al contesto entity
				context.Palestras.Add(palestra);
				context.SaveChanges();
				context.Dispose();
			}

		}

		private void AggiungiAttrezzo_Nome(string text)
		{
			using (var context = new UdemyEseDbContext())
			{
				var attrezzo = new Attrezzo()
				{
					Name = text,
				};
				// Aggiungi l'oggetto Attrezzo al contesto entity
				context.Attrezzos.Add(attrezzo);
				context.SaveChanges();
				context.Dispose();
			}
		}

		// Mostra location della palestra selezionata nella textbox.
		private void MostraPalestraInTextbox(int palestraId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					
					string nomeTabellaPalestra = "Palestra";
					var palestraChosen = context.Palestras.FromSqlRaw<Palestra>($"SELECT * FROM {nomeTabellaPalestra} WHERE Id = @PalestraId", new SqlParameter("@PalestraId", palestraId)).FirstOrDefault();
					if (palestraChosen != null)
					{
						MyTextBox.Text = palestraChosen.Location;
					}

				}
			}
			catch (Exception e)
			{
				//MessageBox.Show(e.Message);
			}
		}

		// Mostra il nome dell'attrezzo selezionato nella Textbox
		private void MostraAttrezzoInTextbox(int attrezzoId)
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					
					string nomeTabellaAttrezzo = "Attrezzo";
					var attrezzoChosen = context.Attrezzos.FromSqlRaw<Attrezzo>($"SELECT * FROM {nomeTabellaAttrezzo} WHERE Id = @AttrezzoId", new SqlParameter("@AttrezzoId", attrezzoId)).FirstOrDefault();
					if (attrezzoChosen != null)
					{
						MyTextBox.Text = attrezzoChosen.Name;
					}

				}
			}
			catch (Exception e)
			{
				//MessageBox.Show(e.Message);
			}
		}

		// Pulizia textbox e selezione listbox quando si clicca in uno spazio vuoto
		private void ClearTextBox(object sender, MouseButtonEventArgs e)
		{
			MyTextBox.Text = string.Empty;  // Cancella il testo nella TextBox
											// Deseleziona gli elementi nelle ListBox
			listPalestre.SelectedItem = null;
			attrezziList.SelectedItem = null;
			tuttiGliAttrezziList.SelectedItem = null;
		}


		// AREA UPDATE DATABASE
		private void UpdateDatabase()
		{
			try
			{
				using (var context = new UdemyEseDbContext())
				{
					// Applica automaticamente tutte le migrazioni pendenti al database
					context.Database.Migrate();
				}
			}
			catch (Exception e)
			{
				// MessageBox.Show(e.Message);
			}

		}

	
	}
}
