class Note {
  const Note({required this.title, required this.body});
  final String title;
  final String body;
}

final notes = [
  const Note(
      title: 'fix httpie-go',
      body:
          'Add header "Accept: */*", nginx seems to block requests if no content negotiation headers present'),
  const Note(
      title: 'learn flutter',
      body:
          'After 1 hour of using flutter it feels great, you gotta learn more'),
];
