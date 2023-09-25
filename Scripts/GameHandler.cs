using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] int paidReqScore;
    [SerializeField] int chanceOfPassing;
    [SerializeField] int adjustment;
    [SerializeField] int wrongDocChance;
    [SerializeField] int[] chances; // 0 - idCard, 1 - snt, 2 - aet, 3 - grant, 4 - receipt
    [SerializeField] GameObject dragAndDrop;
    [SerializeField] GameObject idCardPrefab;
    [SerializeField] GameObject aetPrefab;
    [SerializeField] GameObject sntPrefab;
    [SerializeField] GameObject grantPrefab;
    [SerializeField] GameObject receiptPrefab;

    [Header("Set Dinamically")]
    [SerializeField] Student currentStudent;
    List<GameObject> currentStudentDocs = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintAllStudentData(currentStudent);
            print(currentStudent.CheckDocs());
        }
    }

    public void NewStudent()
    {
        if (currentStudent != null)
        {
            currentStudent = null;
        }
        if(currentStudentDocs.Count > 0)
        {
            for(int i = 0; i < currentStudentDocs.Count; i++)
            {
                Destroy(currentStudentDocs[i]);
            }
            currentStudentDocs.Clear();
        }
        Student newStudent = GenerateNewRandomStudent();
        

        if (newStudent.IdCard.IsHave)
        {
            GameObject idcard = Instantiate(idCardPrefab, GenerateRandomSpawnPos(), Quaternion.identity, dragAndDrop.transform) as GameObject;
            idcard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            currentStudentDocs.Add(idcard);
        }
        if (newStudent.Aet.IsHave)
        {
            GameObject aet = Instantiate(aetPrefab, GenerateRandomSpawnPos(), Quaternion.identity, dragAndDrop.transform);
            aet.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            currentStudentDocs.Add(aet);
        }
        if (newStudent.SntSertificate.IsHave)
        {
            GameObject snt = Instantiate(sntPrefab, GenerateRandomSpawnPos(), Quaternion.identity, dragAndDrop.transform);
            snt.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            currentStudentDocs.Add(snt);
        }
        if (newStudent.GrantSertificate.IsHave)
        {
            GameObject grant = Instantiate(grantPrefab, GenerateRandomSpawnPos(), Quaternion.identity, dragAndDrop.transform);
            grant.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            currentStudentDocs.Add(grant);
        }
        if (newStudent.Receipt.IsHave)
        {
            GameObject receipt = Instantiate(receiptPrefab, GenerateRandomSpawnPos(), Quaternion.identity, dragAndDrop.transform);
            receipt.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            currentStudentDocs.Add(receipt);
        }

        currentStudent = newStudent;
        print("New student");
    }

    Student GenerateNewRandomStudent()
    {
        bool isMale = (Random.Range(0, 2) == 1);

        string[] names = GenerateRandomName(isMale);
        string[] wrongName = GenerateRandomName(isMale);
        int index = Random.Range(0, 3);
        for(int i = 0; i < 3; i++)
        {
            if (i == index) continue;
            wrongName[i] = names[i];
        }

        bool isGrant = (Random.Range(0, 2) == 1);

        int sntScore = GenerateRandomScore(isGrant);

        Document idCard = new Document("ID card", names, IsHaveRandomizer(0));
        Document sntSertificate = new Document("Snt sertificate", (WrongChance() ? wrongName : names),  IsHaveRandomizer(1));
        Document aet = new Document("AET", (WrongChance() ? wrongName : names), IsHaveRandomizer(2));
        Document grantSertificate = new Document("Grant sertificate", (WrongChance() ? wrongName : names), (isGrant ? IsHaveRandomizer(3) : false));
        Document receipt = new Document("Payment receipt", (WrongChance() ? wrongName : names), (isGrant ? false : IsHaveRandomizer(4)));

        Student newStudent = new Student(names, isMale, sntScore, isGrant, idCard, sntSertificate, aet, grantSertificate, receipt);

        return newStudent;
    }

    List<string> GetNamesList(string fileName)
    {
        string path = "Assets/Students/" + fileName;
        StreamReader namesStream = new StreamReader(path);
        List<string> namesList = new List<string>();
        while(!namesStream.EndOfStream) namesList.Add(namesStream.ReadLine());
        namesStream.Close();
        return namesList;
    }

    string[] GenerateRandomName(bool isMale)
    {
        int[] nameIndexes = new int[3];
        do
        {
            nameIndexes[0] = Random.Range(0, 3);
            nameIndexes[1] = Random.Range(0, 3);
            nameIndexes[2] = Random.Range(0, 3);
        } while (nameIndexes[0] == nameIndexes[1] || nameIndexes[1] == nameIndexes[2]);

        string[] names = new string[3];
        names[0] = GetNamesList("Surnames.txt")[nameIndexes[0]] + (isMale ? "" : "a");
        names[1] = GetNamesList(isMale ? "MaleNames.txt" : "FemaleNames.txt")[nameIndexes[1]];
        names[2] = GetNamesList("MaleNames.txt")[nameIndexes[2]] + (isMale ? "ovich" : "ovna");

        return names;
    }

    int GenerateRandomScore(bool isGrant)
    {
        if (isGrant) return Random.Range(96, 135);
        bool isScoreEnough = (chanceOfPassing >= Random.Range(0, 101));
        if (isScoreEnough) return Random.Range(paidReqScore, paidReqScore + adjustment + 1);
        return Random.Range(paidReqScore - adjustment, paidReqScore);
    }

    bool IsHaveRandomizer(int index) => (chances[index] >= Random.Range(0, 101));

    bool WrongChance() => (wrongDocChance >= Random.Range(0, 101));

    Vector3 GenerateRandomSpawnPos()
    {
        float x = Random.Range(-9f, -1.5f);
        float y = Random.Range(-1.5f, -3.3f);
        float z = 0;
        return new Vector3(x, y, z);
    }

    void PrintAllStudentData(Student student)
    {
        if (student == null) return;
        string output = "";
        output += "Name: " + student.Name[0] + " " + student.Name[1] + " " + student.Name[2] + "\n";
        output += "IsMale: " + student.IsMale + "\n";
        output += "IsGrant: " + student.IsGrant + "\n";
        output += "SntScore, sertificate, owner: " + student.SntScore + " " + student.SntSertificate.IsHave + " " + student.SntSertificate.OwnerName[1] + "\n";
        output += "AET, owner: " + student.Aet.IsHave + " " + student.Aet.OwnerName[1] + "\n";
        output += "Grant: " + student.GrantSertificate.IsHave + " " + student.GrantSertificate.OwnerName[1] + "\n";
        output += "Receipt: " + student.Receipt.IsHave + " " + student.Receipt.OwnerName[1];

        print(output);
    }

    class Student
    {
        #region fields
        // 0 - Фамилия, 1 - Имя, 2 - Отчество
        string[] name = new string[3];
        bool isMale;

        int sntScore; //Баллы за ент
        bool isGrant; //true - на грант (нужен сертификат), false - платка (нужен чек)

        Document idCard;
        Document sntSertificate;
        Document aet;
        Document grantSertificate;
        Document receipt;
        #endregion fields

        #region constructors
        public Student(string[] name, bool isMale, int sntScore, bool isGrant)
        {
            this.name[0] = name[0];
            this.name[1] = name[1];
            this.name[2] = name[2];
            this.isMale = isMale;
            this.sntScore = sntScore;
            this.isGrant = isGrant;
        }

        public Student(string[] name, bool isMale, int sntScore, bool isGrant, Document idCard, Document sntSertificate, Document aet, Document grantSertificate, Document receipt) : this(name, isMale, sntScore, isGrant)
        {
            this.name[0] = name[0];
            this.name[1] = name[1];
            this.name[2] = name[2];
            this.isMale = isMale;
            this.sntScore = sntScore;
            this.isGrant = isGrant;
            this.idCard = idCard;
            this.sntSertificate = sntSertificate;
            this.aet = aet;
            this.grantSertificate = grantSertificate;
            this.receipt = receipt;
        }
        #endregion constructors

        #region gettersetters
        public string[] Name
        {
            get { return name; }
        }
        public bool IsMale
        {
            get { return isMale; }
        }
        public int SntScore
        {
            get { return sntScore; }
        }
        public bool IsGrant
        {
            get { return isGrant; }
        }
        public Document IdCard
        {
            get { return idCard; }
        }
        public Document SntSertificate
        {
            get { return sntSertificate; }
        }
        public Document Aet
        {
            get { return aet; }
        }
        public Document GrantSertificate
        {
            get { return grantSertificate; }
        }
        public Document Receipt
        {
            get { return receipt; }
        }
        #endregion gettersetters

        #region methods
        public bool CheckDocs()
        {
            if (isGrant)
            {
                if (!idCard.IsHave || !aet.IsHave || !sntSertificate.IsHave || !grantSertificate.IsHave) return false;
                for(int i = 0; i < 3; i++)
                {
                    if (name[i] != idCard.OwnerName[i]) return false;
                    if (name[i] != aet.OwnerName[i]) return false;
                    if (name[i] != sntSertificate.OwnerName[i]) return false;
                    if (name[i] != grantSertificate.OwnerName[i]) return false;
                }
                return true;
            }
            else
            {
                if (!idCard.IsHave || !aet.IsHave || !sntSertificate.IsHave || !receipt.IsHave) return false;
                for (int i = 0; i < 3; i++)
                {
                    if (name[i] != idCard.OwnerName[i]) return false;
                    if (name[i] != aet.OwnerName[i]) return false;
                    if (name[i] != sntSertificate.OwnerName[i]) return false;
                    if (name[i] != receipt.OwnerName[i]) return false;
                }
                return true;
            }
        }
        #endregion methods
    }

    class Document
    {
        string docName; // название дока (удостоверение, ает и тд)
        string[] ownerName;
        bool isHave; // Имеется ли документ у студента

        public Document(string docName, string[] ownerName, bool isHave)
        {
            this.docName = docName;
            this.ownerName = ownerName;
            this.isHave = isHave;
        }

        public string DocName
        {
            get { return docName; }
        }
        public string[] OwnerName
        {
            get { return ownerName; }
        }
        public bool IsHave
        {
            get { return isHave; }
        }
    }
}
