<p align="center">
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/MobileApp/Icons/Icon.png" alt="image" width="500" height="400"/>
</p>

# Αστeκό ΚΤΕΛ

Το Αστeκό ΚΤΕΛ είναι μια εφαρμογή που αναπτύχθηκε στα πλαίσια της εξαμηνιαίας στο προπτυχιακό μάθημα της Τεχνολογίας Λογισμικού του τμήματος Μηχανικών Η/Υ και Πληροφορικής του Πανεπιστημίου Πατρών. Η ιδέα της ομάδας μας ήταν η πλήρης ψηφιοποίηση των διαδικασιών που αφορούν το αστικό ΚΤΕΛ (σε πρώτη φάση στην πόλη της Πάτρας), δίνοντας ιδιαίτερη έμφαση στην αποφυγή του συνωστισμού των πολιτών στα λεωφορεία. Μια τέτοια πρόταση είναι κάτι το επαναστατικό, καθώς κάτι παρόμοιο δεν έχει υλοποιηθεί στην πόλη της Πάτρας και επίσης θα λέγαμε αναγκαίο, καθώς ο συνωστισμός στα μέσα μαζικής μεταφοράς (ΜΜΜ) αποτελεί έναν από τους σημαντικότερους παράγοντες στη διασπορά του SARS-CoV-2. Πέρα από την ευκολία και το γενικότερο όφελος που παρέχει μια τέτοια ψηφιοποίηση, η καινοτομία που προτείνουμε ως προς την αποφυγή του συνωστισμού, θα λέγαμε πως είναι απαραίτητη για οποιαδήποτε επιχείρηση, η οποία κατέχει υψηλή κοινωνική συνείδηση και σέβεται πάνω από όλα την ασφάλεια των πελατών της. Εξάλλου, στις δύσκολες συνθήκες που βιώνουμε λόγω της πανδημίας, ο καθοριστικός παράγοντας στη προτίμηση ενός πελάτη είναι η ασφάλεια του.  
Όσον αφορά την ιδέα για την αποφυγή του συνωστισμού, βασιζόμαστε στη παρατήρηση πως καθημερινά υπάρχουν υπερσυγκεντρώσεις πολιτών στα λεωφορεία σε ορισμένα δρομολόγια, ενώ άλλα δρομολόγια είναι οριακά “νεκρά”. Αυτό συμβαίνει διότι η λογική της εμπειρικής πρόβλεψης για το πότε μια ώρα είναι ώρα αιχμής ή ένα δρομολόγιο είναι “hot”, στην οποία και βασίζεται η κατανομή των δρομολογίων, πολλές φορές πέφτει εκτός. Άμεση συνέπεια αυτού είναι ο επικίνδυνος συνωστισμός, ο οποίος πρέπει με κάποιον τρόπο να αντιμετωπιστεί. Την λύση δίνει η εφαρμογή Αστeκό ΚΤΕΛ, η οποία δεν βασίζεται στην εμπειρία για την κατανομή των δρομολογίων, αλλά στις πραγματικές απαιτήσεις των πελατών. Όπως είναι λογικό μια κατανομή βασισμένη στις απαιτήσεις και ανάγκες των πελατών οδηγεί στην αποφυγή του συνωστισμού, μέσω της πύκνωσης των δρομολογίων όποτε αυτό χρειάζεται πραγματικά και την αραίωση τους όταν αυτά είναι αχρείαστα. Έτσι, πέρα από την αποφυγή του συνωστισμού, γίνεται και εξοικονόμηση βενζίνης και γενικότερα πόρων, καθώς τα κενά δρομολόγια αποφεύγονται, με άμεσο αποτέλεσμα το οικονομικό όφελος.

## Demo της εφαρμογής

[Πατήστε εδώ](https://www.youtube.com/watch?v=w7eIrr64G9Y)

## Μέλη ομάδας

[Γερογιάννης Δημήτριος](https://github.com/dimitrisgerog)  
[Κύρος Στέργιος](https://github.com/stergioskyros)  
[Στρατηγόπουλος Γεώργιος](https://github.com/gstratigopoulos96)  
[Τριπολίτης Ιωάννης-Νικόλαος](https://github.com/JohnTripGR) 

## Γλώσσα προγραμματισμού

Η γλώσσα προγραμματισμού στην οποία γράφτηκε η εφαρμογή είναι η Visual C# της Microsoft.

## Framework

Το Framework στο οποίο γράφτηκε η εφαρμογή είναι το .net Core 3.1. Επίσης το GUI κατασκευάστηκε μέσω της τεχνολογίας Windows Forms που προσφέρει η γλώσσα.

## Σχετικά με την εφαρμογή

Οι ρόλοι της εφαρμογής είναι οι ακόλουθοι:
1. Πελάτης-Επιβάτης
2. Οδηγός λεωφορείων
3. Προϊστάμενος
4. Υπεύθυνος κατανομής δρομολογίων
5. Υπεύθυνος διασφάλισης υπηρεσιών

Η εφαρμογή αποτελείται από 3 solutions.

1. Classes  
2. DesktopApp  
3. MobileApp

Το solution **Classes** περιέχει τις κλάσεις που χρησιμοποιούνται στην εφαρμογή. Το solution **DesktopApp** περιέχει το GUI των ρόλων *προϊστάμενος*, *υπεύθυνος κατανομής δρομολογίων*, *υπεύθυνος διασφάλισης υπηρεσιών*. Τέλος το solution **MobileApp** περιέχει το GUI των ρόλων *οδηγός λεωφορείων* και *πελάτης-επιβάτης*.

## Οδηγίες για να μπορέσετε να τρέξετε την εφαρμογή

**Προσοχή: Η εφαρμογή τρέχει μόνο στα windows 10.**

### 1ο στάδιο
Θα πρέπει αρχικά να διαθέτε εγκατεστημένη την **MySQL**.

### 2ο στάδιο
Θα πρέπει να φτιάξετε την βάση με τη χρήση του κώδικα της βάσης που θα βρείτε στο [αρχείο](https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/databasesrc.sql) της βάσης.

### 3ο στάδιο
Θα πρέπει να εγκαταστήσετε το [Visual Studio 2019 community](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=16) και συγκεκριμένα το framework .net Core 3.1 κατα την εγκατάσταση πακέτων για να μπορέσετε να τρέξετε την εφαρμογή.

<p align="center">
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/installer.png" alt="image"/>
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/installdotnetdesktopdevelopment.png" alt="image"/>
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/installnetcore3.1.png" alt="image"/>
</p>

### 4ο στάδιο
Θα πρέπει να φτιάξε ένα αρχείο connectionstring.txt στην επιφάνεια εργασίας σας όπου θα περιέχει το ακόλουθο string ανάλογα με το όνομα του server, το userid, το password που έχετε καταχωρήσει στην MySQL και υποχρεωτικά όνομα βάσης **project_db**. Σε κάθε άλλη περίπτωση δεν θα μπορείτε να τρέξετε την εφαρμογή.

```
server=localhost;userid=root;password=1234;database=project_db
```

<p align="center">
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/Capture.PNG" alt="image"/>
</p>

### 5ο στάδιο
Θα πρέπει να κάνετε τα εξής βήματα:

1. Ανοίγετε το αρχείο **SoftwareTechnologyProject.sln** με τη χρήση του Visual Studio 2019.
1. Build (F6).
2. Επιλογή Debug ή Release mode.
3. Επιλογή **DesktopApp** ή **MobileApp**.
4. Run.

Παρακάτω φαίνονται μερικές φωτογραφίες που δείχνουν τη σειρά των βημάτων.

<p align="center">
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/openproject.png" alt="image"/>
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/build.jpg" alt="image"/>
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/debug-release%20mode.png" alt="image"/>
  <img src="https://github.com/gstratigopoulos96/Asteko_KTEL/blob/master/Photos/select%20project.png" alt="image"/>
</p>

Εναλλακτικά μετά την ολοκλήρωση των παραπάνω βημάτων για τα 2 projects (DekstopApp, MobileApp) θα μπορείτε να ανοίξετε την εφαρμογή χωρίς τη χρήση του Visual Studio 2019 απλά κάνοντας execute τα αρχεία **DesktopApp.exe** ή **MobileApp.exe** που θα δημιουργηθούν στους παρακάτω φακέλους ανάλογα με το αν έχετε κάνει Run σε Debug ή Release mode.

`...\DesktopApp\bin\Debug\netcoreapp3.1\`  
`...\DesktopApp\bin\Release\netcoreapp3.1\`  
`...\MobileApp\bin\Debug\netcoreapp3.1\`  
`...\MobileApp\bin\Release\netcoreapp3.1\`  

#### Προσοχή
Δεν θα μπορέσετε να τρέξετε την εφαρμογή μέσω των **DesktopApp.exe** ή **MobileApp.exe** αν τα μετακινήσετε σε κάποιο άλλο φάκελο όπου δεν υπάρχουν τα υπόλοιπα αρχεία του αντίστοιχου φακέλου `...\netcoreapp3.1` από τον οποίο πήρατε το executable.

## License
[MIT](https://choosealicense.com/licenses/mit/)
