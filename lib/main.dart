import 'package:flutter/material.dart';

void main() {
  runApp(const App());
}

class App extends StatelessWidget {
  const App({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        debugShowCheckedModeBanner: false,
        title: 'Sticky Notes',
        home: Scaffold(
          appBar: AppBar(
            title: const Text('Stick Notes'),
          ),
          body: Column(children: const [SizedBox(height: 8), NoteCardList()]),
        ));
  }
}

class Note {
  final String title;
  final String body;
  const Note(this.title, this.body);
}

final notes = [
  const Note('fix httpie-go',
      'Add header "Accept: */*", nginx seems to block requests if no content negotiation headers present'),
  const Note('learn flutter',
      'After 1 hour of using flutter it feels great, you gotta learn more'),
];

class NoteCardList extends StatelessWidget {
  const NoteCardList({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(
      children: notes.map((note) => NoteCard(note: note)).toList(),
    );
  }
}

class NoteCard extends StatelessWidget {
  final Note note;

  const NoteCard({required this.note, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Card(
      color: const Color.fromARGB(255, 255, 228, 241),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: <Widget>[
          ListTile(
              leading: const Icon(Icons.info_outline),
              title: Text(
                note.title,
                style: const TextStyle(fontWeight: FontWeight.bold),
              ),
              subtitle: Text(note.body)),
          Row(
            mainAxisAlignment: MainAxisAlignment.end,
            children: <Widget>[
              TextButton(
                child: const Text(
                  'Delete Note',
                  style: TextStyle(color: Colors.red),
                ),
                onPressed: () {/* ... */},
              ),
              const SizedBox(width: 8),
              TextButton(
                child: const Text('Open Note'),
                onPressed: () {/* ... */},
              ),
              const SizedBox(width: 8),
            ],
          ),
        ],
      ),
    );
  }
}
