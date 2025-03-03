using VDF3_Solution3;
using System;
using System.Windows.Forms;

public class TimerManager
{
    private Timer _timer;
    public Action OnTimerTickAction;  // Delegate để lưu hàm callback

    public TimerManager()
    {
        // Khởi tạo Timer
        _timer = new Timer();
        _timer.Interval = 5000; // 5 giây
        _timer.Tick += Timer_Tick; // Đăng ký sự kiện Tick
    }

    // Bắt đầu Timer
    public void Start()
    {
        _timer.Start();
    }

    // Dừng Timer
    public void Stop()
    {
        _timer.Stop();
    }

    // Xử lý sự kiện Tick của Timer
    private async void Timer_Tick(object sender, EventArgs e)
    {
        // Kiểm tra xem delegate có được gán không
        OnTimerTickAction?.Invoke(); // Gọi hàm delegate
    }
}
