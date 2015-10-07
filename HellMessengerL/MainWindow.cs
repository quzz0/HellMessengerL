using System;
using Gtk;
using HellMessengerL;

//smellcode

public partial class MainWindow: Gtk.Window
{	
	public Gtk.ListStore messageListStore;
	public Chat chat = new Chat();

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		initSettings ();
		initEvents ();
		initChat ();
	}

	public void initSettings() {
		this.Title = "HellMessenger L";
	}

	public void initEvents() {
		sendButton.Clicked += OnSendEvent;
		connectButton.Clicked += OnConnectEvent;
	}

	public void initChat() {
		Gtk.TreeView chatTree = new Gtk.TreeView ();
		messagesPanel.Add (chatTree);

		Gtk.TreeViewColumn userColumn = new Gtk.TreeViewColumn ();
		userColumn.Title = "User";

		Gtk.CellRendererText userCell = new Gtk.CellRendererText ();
		userColumn.PackStart (userCell, true);

		Gtk.TreeViewColumn messageColumn = new Gtk.TreeViewColumn ();
		messageColumn.Title = "Message";

		Gtk.CellRendererText messageCell = new Gtk.CellRendererText ();
		messageColumn.PackStart (messageCell, true);

		chatTree.AppendColumn (userColumn);
		chatTree.AppendColumn (messageColumn);

		userColumn.AddAttribute (userCell, "text", 0);
		messageColumn.AddAttribute (messageCell, "text", 1);

		messageListStore = new Gtk.ListStore (typeof (string), typeof (string));

		//messageListStore.AppendValues ("nullbyte", "Hello World");

		chatTree.Model = messageListStore;
		messagesPanel.ShowAll ();
	}

	protected void OnSendEvent(object obj, EventArgs args)
	{
		string username = usernameBox.Text;
		string text = sendText.Buffer.Text;
		if(username.Length > 0 && text.Length > 0 && 
		   chat.sendMessage(new Message() { Username =  username, Text = text }))
			messageListStore.AppendValues (username, text);
		
	}

	protected void OnConnectEvent(object obj, EventArgs args)
	{
		//enable chat
		messagesPanel.Sensitive = true;
		sendText.Sensitive = true;
		sendButton.Sensitive = true;
		usernameBox.Sensitive = false;
		connectButton.Sensitive = false;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
