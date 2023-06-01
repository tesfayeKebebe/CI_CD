namespace SharedComponent.StateManagement;

public class NotificationStateService
{
    public event Action? OnPatientFileChanged;
    private void NotifyPatientFile() => OnPatientFileChanged?.Invoke();
    public event Action? OnDraftChanged;
    private void NotifyDraft() => OnDraftChanged?.Invoke();
    public event Action? OnProgressChanged;
    private void NotifyOnProgress() => OnProgressChanged?.Invoke();
    public int PatientFile { get; set; } = 0;
    public int Draft { get; set; } = 0;
    public int Progress{ get; set; } = 0;

    public void SetPatientFile(int count)
    {
        if (PatientFile == count)
        {
            return;
        }
        PatientFile = count;
        NotifyPatientFile();
    }
    public void SetDraft(int count)
    {
        if (Draft == count)
        {
            return;
        }
        Draft = count;
        NotifyDraft();
    }
    public void SetOnProgress(int count)
    {
        if (Progress == count)
        {
            return;
        }
        Progress = count;
        NotifyOnProgress();
    }


}