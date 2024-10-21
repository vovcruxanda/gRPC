# gRPC
gRPC este o platformă modernă de înaltă performanță, derivată din vechiul protocol RPC (Remote Procedure Call), facilitând mesageria între clienți și serviciile de backend.

**Avantajele HTTP/2**

- Comprimarea antetului
- Multiplexare (trimiterea mai multor solicitări simultan)
- Conexiune unică pentru cereri multiple
- Împingerea răspunsurilor multiple către client
- Folosirea formatului binar în loc de text

**Beneficiile gRPC**

Utilizarea HTTP/2 ca protocol de transport, cu funcții avansate:

- Protocol de încadrare binar pentru transportul de date.
- Suport pentru multiplexare, permițând trimiterea mai multor cereri paralele printr-o singură conexiune.
- Comunicație bidirecțională full duplex pentru a trimite cererile clientului și răspunsurile serverului simultan.
- Streaming încorporat pentru transmiterea asincronă a cererilor și răspunsurilor.
- Comprimarea antetului care reduce utilizarea rețelei.

---
Sistemul gRPC implementat în laboratorul dat se bazează pe arhitectura client-server, permițând comunicația eficientă între diferite servicii prin intermediul protocolului gRPC. Acesta este un protocol de apeluri de procedură la distanță (RPC) care utilizează HTTP/2 pentru transmiterea rapidă a mesajelor și Protobuf (Protocol Buffers) ca format de serializare a datelor.

HTTP/2 este o versiune îmbunătățită a protocolului HTTP/1.1, concepută pentru a face transferul datelor pe web mai rapid și mai eficient. Caracteristici cheie sunt:

- Multiplexare: Permite trimiterea mai multor cereri și răspunsuri prin aceeași conexiune simultan.
- Compresia headerelor: Reduce dimensiunea datelor trimise.
- Server Push: Serverul poate trimite resurse către client înainte ca acestea să fie solicitate.
- Protocol binar: Mai rapid și mai eficient decât protocolul bazat pe text.

Protobuf este un format de serializare a datelor dezvoltat de Google, folosit pentru a comprima și a transfera date într-un mod eficient. Acesta funcționează prin definirea structurii datelor într-un fișier .proto, iar apoi acesta este transformat în cod care poate fi folosit pentru a converti datele în format binar și invers. Caracteristici cheie sunt:

- Compact și eficient: Datele sunt stocate într-un format binar, ocupând mai puțin spațiu.
- Compatibilitate între limbaje: Funcționează cu mai multe limbaje de programare.
- Compatibilitate retroactivă: Permite modificări în structură fără a afecta sistemele existente.

Este utilizat în comunicație între microservicii și stocarea datelor.

### **Componentele Sistemului**

#### Proiectul Broker:

Program.cs: Acesta este punctul de intrare al aplicației Broker. Acesta configurează și pornește serverul gRPC, stabilind adresa brokerului.

Startup.cs: Această clasă definește serviciile și middleware-ul aplicației. Aici se adaugă serviciile gRPC și se configurează rutele pentru clienți.

#### Proiectul Receiver:

Program.cs: Punctul de intrare pentru aplicația Receiver. Acesta inițializează serverul și gestionează subscrierea la broker.

Startup.cs: Similar cu proiectul Broker, configurează serviciile gRPC și rutele pentru gestionarea cererilor.

#### Namespace-ul Common:

EndpointsConstants.cs: Conține constantele pentru adresele brokerului și subscrierilor. Aceste adrese sunt utilizate în ambele aplicații pentru a stabili conexiuni corecte.

### Serviciile:

MessageStorageService și ConnectionStorageService: Aceste servicii sunt responsabile pentru gestionarea mesajelor și a conexiunilor între clienți și broker. Ele sunt înregistrate ca singletoni pentru a asigura că aceeași instanță este utilizată în întreaga aplicație.

SenderWorker: Un serviciu de fundal care gestionează trimiterea mesajelor către abonați.

### Funcționarea Sistemului

Configurarea Serverului:

Atunci când aplicația Broker pornește, aceasta configurează serverul gRPC să asculte la adresa specificată în EndpointsConstants.BrokerAddress. Serverul va gestiona cererile de la clienți prin intermediul metodelor definite în serviciile gRPC.

Subscrierea la Broker:

Aplicația Receiver pornește și se abonează la broker. Utilizatorul este solicitat să introducă un topic pentru care dorește să primească mesaje. Aceasta se face prin apelul metodei Subscribe, care trimite o cerere către broker, specificând topicul și adresa unde receiver-ul ascultă.

Transmiterea Mesajelor:

Odată ce un Receiver se abonează, brokerul poate transmite mesaje către acesta atunci când acestea sunt disponibile. Mesajele sunt gestionate prin MessageStorageService, care se ocupă de stocarea și gestionarea mesajelor primite.

Răspunsuri și Feedback:

Atunci când Receiver-ul se abonează cu succes, brokerul returnează un răspuns care indică succesul operațiunii. Acest feedback este util pentru utilizator, confirmând că subscrierea a fost efectuată cu succes.
