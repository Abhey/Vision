/*using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class RowEntry{
  public double lat;
  public double lon;
  public string data;

  public RowEntry(double _lat, double _lon, string _data){
    lat = _lat;
    lon = _lon;
    data = _data;
  }

}

public class FirebaseHandler : MonoBehaviour {

  DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

  void Start() {

    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available) {
        InitializeFirebase();
      } else {
        Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });

  }

  protected virtual void InitializeFirebase() {

    FirebaseApp app = FirebaseApp.DefaultInstance;
    app.SetEditorDatabaseUrl("https://vision-avatar.firebaseio.com/");
    if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

  }

  public void GetRow(){

    FirebaseDatabase.DefaultInstance
    .GetReference("Rows")
    .GetValueAsync().ContinueWith(task => {
      if (task.IsFaulted) {
        // Handle the error...
      }
      else if (task.IsCompleted) {
        DataSnapshot snapshot = task.Result;
        // Do something with snapshot...
        Debug.Log(snapshot);
      }
    });

  }

 public void AddRow(_lat , _lon, _data){

    string key = FirebaseDatabase.DefaultInstance.GetReference("Rows").Push().Key;
    RowEntry e = new RowEntry(_lat, _lon, _data);
    Dictionary<string, Object> eVal = e.ToDictionary();

    Dictionary<string, Object> childUpdates = new Dictionary<string, Object>();
    childUpdates[key] = eValues;
    FirebaseDatabase.DefaultInstance.GetReference("Rows").UpdateChildrenAsync(childUpdates);

  }

}*/