import 'package:flutter/material.dart';
import '../models/note.dart';

class HomePage extends StatefulWidget {
  const HomePage({Key? key, required this.notes}) : super(key: key);

  final List<Note> notes;

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  // @TODO: Actions that change the state...

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Stick Notes'),
      ),
      body: Column(children: [
        const SizedBox(height: 8),
        ...notes.map((note) => NoteCard(note: note)).toList(),
      ]),
      floatingActionButton: FloatingActionButton(
        onPressed: () => {},
        tooltip: 'New Note',
        child: const Icon(Icons.add),
      ),
    );
  }
}

class NoteCard extends StatelessWidget {
  final Note note;

  const NoteCard({required this.note, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return InkWell(
        onTap: () {
          debugPrint('Card ${note.title} tapped.');
        },
        child: Card(
          color: Colors.amberAccent,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: <Widget>[
              ListTile(
                  title: Text(
                    note.title,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Text(note.body,
                      style: const TextStyle(color: Colors.black))),
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
                  const Icon(Icons.delete, size: 24.0, color: Colors.grey),
                  const SizedBox(width: 8),
                ],
              ),
            ],
          ),
        ));
  }
}
