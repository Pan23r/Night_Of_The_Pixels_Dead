using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectIstantiator_Manager : MonoBehaviour
{
    //AGGIUNGERE IL NOME DELL'OGGETTO DA RACCOGLIERE
    #region Object to take (FOR PLAYERMOVEMENT)
    public static bool ObjToTake(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Gun":
                return true;

            case "KeyLab":
                return true;

            case "KeySally":
                return true;

            case "Floppy":
                return true;

            case "Medallion":
                return true;

            case "PieceOfCloth":
                return true;
        }

        for(int i = 0; i < ObjectIsTakeDelete_manager.munitionIsTaken.Length; i++)
        {
            if (collision.gameObject.name == $"EagleGold_{i}")
                return true;
        }

        return false;
    }
    #endregion

    //REMEMBER: PER DISTRUGGERE OGGETTO CHE SI PRENDE USARE LA CLASSE OObjectTakeDelete_manager (RICORDA CHE LO SCRIPT VA ASSOCIATO AL GAMEOBJECT A CUI SI RIFERISCE)

    //AGGIUNGERE IL NOME PER CREARE L'OGGETTO NELL'INVENTARIO
    #region Return Obj
    public static bool ReturnObj()
    {
        switch (PlayerMovement.ObjectCollideName)
        {
            case "Gun":
                return ObjectIsTakeDelete_manager.gunIsTaken = ObjectInInventory_Manager.PutObject(Translation_Manager.nameGun, "Gun_Obj","Gun", true, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj, Shooter_Manager.gunAmmunition, -10, -110);

            /*case "KeyLab":
                return ObjectIsTakeDelete_manager.keyLabIsTaken = ObjectInInventory_Manager.PutObject("Chiave Laboratorio", "Key_Lab", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj);*/

            case "KeySally":
                return ObjectIsTakeDelete_manager.keySallyIsTaken = ObjectInInventory_Manager.PutObject(Translation_Manager.nameSallyKey,"KeySallysRoom_Obj" ,"Key_BedRoomGirl", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj);

            case "Floppy":
                return ObjectIsTakeDelete_manager.FloppyIsTaken = ObjectInInventory_Manager.PutObject(Translation_Manager.nameFloppy, "FloppyDisk_Obj","Floppy_Disk", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj);

            case "Medallion":
                return ObjectIsTakeDelete_manager.MedallionIsTaken = ObjectInInventory_Manager.PutObject(Translation_Manager.nameMedallion, "Medallion_Obj", "Medaglia_Vitruviana", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj);

            case "PieceOfCloth":
                return ObjectIsTakeDelete_manager.pieceOfClothIsTaken = ObjectInInventory_Manager.PutObject(Translation_Manager.namepieceOfCloth, "PieceOfCloth_Obj", "Patch_With_Needle", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj, OpenSaveMenu_Manager.numberOfPOC, -10, -110);

        }

        for (int i = 0; i < ObjectIsTakeDelete_manager.munitionIsTaken.Length; i++)
        {
            if (PlayerMovement.ObjectCollideName == $"EagleGold_{i}" && !ObjectInInventory_Manager.SearchObj("Eagle_Obj"))
            {
                Shooter_Manager.ammunitionBox = (Shooter_Manager.ammunitionBox <= 0) ? Shooter_Manager.maxAmmunitionBox : Shooter_Manager.ammunitionBox;
                return ObjectIsTakeDelete_manager.munitionIsTaken[i] = ObjectInInventory_Manager.PutObject(Translation_Manager.nameAmmunitionGun, "Eagle_Obj","Eagle", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj, Shooter_Manager.ammunitionBox, -10, -110);
            }                
            else if (PlayerMovement.ObjectCollideName == $"EagleGold_{i}" && ObjectInInventory_Manager.SearchObj("Eagle_Obj"))
            {
                Shooter_Manager.ammunitionBox += Shooter_Manager.maxAmmunitionBox;
                TextBoxObjectTake_Manager.ActivateTextBox(Translation_Manager.nameAmmunitionGun, true);
                return ObjectIsTakeDelete_manager.munitionIsTaken[i] = true;
            }
        }

        return false;
    }
    #endregion

    //AGGIUNGERE NOME E DESCRIZIONE PER QUANDO SI ESAMINA L'OGGETTO
    #region Return Examination Text
    public static string ReturnExaminationText(string nameObject)
    {
        switch (nameObject)
        {
            case "Gun_Obj":
                return Translation_Manager.gunExamination;

            case "KeySallysRoom_Obj":
                return Translation_Manager.sallysKeyExamination;

            case "KeyLab_Obj":
                return Translation_Manager.labKeyExamination;

            case "Eagle_Obj":
                return Translation_Manager.ammunitionExamination;

            case "FloppyDisk_Obj":
                    return Translation_Manager.floppyExamination;

            case "FloppyDiskWithMusicSheet_Obj":
                return Translation_Manager.floppyWhithMusicSheetExamination;

            case "MusicSheet_Obj":
                return Translation_Manager.musicSheetExamination;
                
            case "Medallion_Obj":
                return Translation_Manager.medallionExamination;

            case "Magnum_Obj":
                return Translation_Manager.magnumExamination;

            case "PieceOfCloth_Obj":
                return Translation_Manager.pieceOfClothExamination;
        }

        return nameObject;
    }
    #endregion

    //AGGIUNGERE LE CONDIZIONI PER COMBINARE GLI OGGETTI
    #region Combine Object
    public static bool CombineObjects(string object1, string object2)
    {
        if (object1 == "Gun_Obj" && object2 == "Eagle_Obj" || object1 == "Eagle_Obj" && object2 == "Gun_Obj")
        {
            if(Shooter_Manager.gunAmmunition < Shooter_Manager.maxGunAmmunition)
            {
                Shooter_Manager.gunAmmunition += Shooter_Manager.ammunitionBox;
                int newAmmonition = Shooter_Manager.gunAmmunition - Shooter_Manager.maxGunAmmunition;
                Shooter_Manager.ammunitionBox = newAmmonition;

                if (Shooter_Manager.gunAmmunition > Shooter_Manager.maxGunAmmunition)
                {
                    Shooter_Manager.gunAmmunition = Shooter_Manager.maxGunAmmunition;
                }

                if(Shooter_Manager.ammunitionBox < 1)
                {
                    ObjectInInventory_Manager.SearchObjAndDelete("Eagle_Obj");
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        return false;
    }
    #endregion

    //AGGIUNGERE DOVE UN OGGETTO SI PUO' USARE (COLLIDER)
    #region Return Usable Object
    public static bool ReturnUsableObj(string nameObject)
    {
        if (!ChoiceOfDifficultyMenu_Manager.easyMode)
        {
            switch (nameObject)
            {
                case "KeySallysRoom_Obj":
                    if (PlayerMovement.ObjectCollideName == "Child_BedRoom")
                        return Change_room_Manager.BedRoomGirlIsOpen = true;
                    else
                        return false;

                case "KeyLab_Obj":
                    if (PlayerMovement.ObjectCollideName == "Studio_room")
                        return Change_room_Manager.LabIsOpen = true;
                    else
                        return false;

                case "FloppyDisk_Obj":
                    if (PlayerMovement.ObjectCollideName == "ComputerLab")
                        return FloppyQuestTrigger_Manager.useFloppy = true;
                    else
                        return false;

                case "FloppyDiskWithMusicSheet_Obj":
                    if (PlayerMovement.ObjectCollideName == "ComputerGirl")
                        return FloppyQuestTrigger_Manager.useFloppy = true;
                    else
                        return false;

                case "MusicSheet_Obj":
                    if (PlayerMovement.ObjectCollideName == "Piano_Text")
                    {
                        FloppyQuestTrigger_Manager.dontOpenGameMenu = true;
                        return FloppyQuestTrigger_Manager.useMusicSheet = true;
                    }
                    else
                        return false;

                case "Medallion_Obj":
                    if (PlayerMovement.ObjectCollideName == "Tomba_Text")
                    {
                        TakePython_Quest_Manager.canTakeColt = true;
                        FMOD_Sound_Manager.PlayMagnumOpenScompSound();
                        return true;
                    }
                    else
                        return false;

                default:
                    return false;
            }
        }
        else
            return false;
        
    }
    #endregion

    //AGGIUNGERE NOME DI UN OGGETTO CHE CAMBIA 
    #region Return Name Of Obj
    public static void ReturnNameOfObj()
    {
        for(int i = 0; i < ObjectInInventory_Manager.myName.Length; i++)
        {
            switch (ObjectInInventory_Manager.nameGameObject[i])
            {
                case "Gun_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameGun;
                    break;

                case "KeySallysRoom_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameSallyKey;
                    break;

                case "FloppyDisk_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameFloppy;
                    break;

                case "Eagle_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameAmmunitionGun;
                    break;

                case "KeyLab_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameLabKey;
                    break;

                case "FloppyDiskWithMusicSheet_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameFloppyWithMusicSheet;
                    break;

                case "MusicSheet_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameMusicSheet;
                    break;
                    
                case "Medallion_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.nameMedallion;
                    break;
                    
                case "PieceOfCloth_Obj":
                    ObjectInInventory_Manager.myName[i] = Translation_Manager.namepieceOfCloth;
                    break;
            }
        }
    }

    #endregion

    //REMEMBER: QUANDO CREI UN OGGETTO CON MUNIZIONI ES. TORNA NELL' Inventory_Manager IN Update PER AGGIORNARE IL CONTO
}
