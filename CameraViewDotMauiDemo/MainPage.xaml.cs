using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using Microsoft.Maui;

namespace CameraViewDotMauiDemo
{
    public partial class MainPage : ContentPage
    {
        private readonly ICameraProvider _cameraProvider;

        public MainPage(ICameraProvider cameraProvider)
        {
            InitializeComponent();
            _cameraProvider = cameraProvider;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            await _cameraProvider.RefreshAvailableCameras(CancellationToken.None);
            MyCamera.SelectedCamera = _cameraProvider.AvailableCameras.
                Where(x => x.Position == CameraPosition.Rear).FirstOrDefault();

            
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            MyCamera.MediaCaptured -= MyCamera_MediaCaptured;
            MyCamera.Handler?.DisconnectHandler();
        }

        private void MyCamera_MediaCaptured(object? sender , CommunityToolkit.Maui.Views.MediaCapturedEventArgs e)
        {
            if (Dispatcher.IsDispatchRequired)
            {
                Dispatcher.Dispatch(() => MyImage.Source = ImageSource.FromStream(() => e.Media));
                return;
            }
                MyImage.Source = ImageSource.FromStream(() => e.Media);
        }

        private async void Capture_Image(object sender, EventArgs e)
        {
            await MyCamera.CaptureImage(CancellationToken.None);
        }

        private async void Back_Camera(object sender, EventArgs e)
        {
            await _cameraProvider.RefreshAvailableCameras(CancellationToken.None);
            MyCamera.SelectedCamera = _cameraProvider.AvailableCameras.
                Where(x => x.Position == CameraPosition.Rear).FirstOrDefault();
        }

        private async void Front_Camera(object sender, EventArgs e)
        {
            await _cameraProvider.RefreshAvailableCameras(CancellationToken.None);
            MyCamera.SelectedCamera = _cameraProvider.AvailableCameras.
                Where(x => x.Position == CameraPosition.Front).FirstOrDefault();
        }

        private void Flash_On_Off(object sender, EventArgs e)
        {
            MyCamera.CameraFlashMode = MyCamera.CameraFlashMode == CameraFlashMode.Off 
                ? CameraFlashMode.On : CameraFlashMode.Off;
        }

        private void Zoom_In(object sender, EventArgs e)
        {
            MyCamera.ZoomFactor += 0.1f;
        }
    }

}
