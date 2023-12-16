DESCRIPTION:
This WPF app manages gyms and gym equipment. It uses SQL server to stock informations.
The Buttons let the user perform CRUD operations on DB.

LISTBOXES

Listbox "Lista Palestre"(Gym's List) : Shows all the gyms registered by the user

Listbox "Attrezzi in Palestra"(Equipment in the Gym) : Shows all the gym equipment allocated in the gym selected from the listbox "Lista Palestre"

Listbox "Tutti gli Attrezzi"(All the Equipment) : Shows all the equipment registered by the user


BUTTONS

DELETE / ADD Buttons
Button "Cancella Palestra"(Delete Gym) : Deletes the gym selected in the listbox "Lista Palestre"(Gym's List)
Button "Cancella Attrezzo"(Delete Equipment) : Deletes from the gym equipment's listbox "Tutti gli Attrezzi"(All the Equipment) the equipment selected in the listbox.
Button "Rimuovi Attrezzo"(Remove Equipment) : Removes from the listbox "Attrezzi in Palestra"(Equipment in the Gym) the selected item.

Button "Aggiungi Palestra"(Add Gym) : Registers the gym written in the textbox and shows it in the "Lista Palestre"(Gym's List) listbox
Button "Aggiungi Attrezzo"(Add Equipment) : Registers the equipment written in the textbox and shows it in the "Tutti gli Attrezzi"(All the Equipment) listbox
Button "Aggiungi alla Palestra"(Add to the Gym) : Adds to the gym selected from the listbox "Lista Palestre"(Gym's List) the equipment selected from the listbox "Tutti gli Attrezzi"(All the Equipment)


UPDATE Buttons
Button "Aggiorna Palestra"(Update Gym) : Updates the selected gym from "Lista Palestre"(Gym's List). The registered name is shown in the textbox, letting the user to modify it
Button "Aggiorna Attrezzo"(Update Equipment) : Updates the selected equipment from "Tutti gli Attrezzi"(All the Equipment). The registered name is shown in the textbox, letting the user to modify it
