using System.Linq;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;
using PetShelter.Application.Domain;

namespace PetShelter.Uix.Views;

public partial class AdoptionRequestsWindow : Window
{
    private readonly IAdoptionRequestService _requestService;

    private record RequestItem(long UserId, long AnimalId, string Display);

    public AdoptionRequestsWindow()
    {
        InitializeComponent();

        _requestService = Injector.CreateInstance<IAdoptionRequestService>();

        LoadRequests();
    }

    private void LoadRequests()
    {
        var listBox = this.FindControl<ListBox>("RequestsListBox");
        if (listBox == null) return;

        var all = _requestService.GetAllRequests();
        var pending = all.Where(r => r.AdoptionStatus == AdoptionStatus.Pending).ToList();

        var userService = Injector.CreateInstance<IUserService>();

        var items = new List<RequestItem>();
        foreach (var req in pending)
        {
            string userStr = "Unknown user";
            try { var u = userService.GetById(req.UserId); userStr = $"{u.Name} {u.Surname}"; } catch { }
            string animalStr = $"Animal #{req.AnimalId}";

            items.Add(new RequestItem(req.UserId, req.AnimalId, $"{userStr} - {animalStr} (requested {req.RequestDate:d})"));
        }
        listBox.ItemsSource = items.Select(i => i.Display).ToList();
        // store mapping via Tag for simple approach
        listBox.Tag = items;
    }

    private RequestItem? GetSelectedItem()
    {
        var listBox = this.FindControl<ListBox>("RequestsListBox");
        if (listBox == null) return null;
        if (listBox.SelectedIndex < 0) return null;
        var items = listBox.Tag as List<RequestItem>;
        if (items == null) return null;
        return items.ElementAtOrDefault(listBox.SelectedIndex);
    }

    private void ApproveButton_Click(object? sender, RoutedEventArgs e)
    {
        var sel = GetSelectedItem();
        if (sel == null) return;

        try
        {
            _requestService.ApproveRequest(sel.UserId, sel.AnimalId);
            LoadRequests();
        }
        catch (Exception ex)
        {
            // show error in simple dialog
            var dlg = new Window { Width = 300, Height = 120, Content = new TextBlock { Text = ex.Message } };
            dlg.ShowDialog(this);
        }
    }

    private void DeclineButton_Click(object? sender, RoutedEventArgs e)
    {
        var sel = GetSelectedItem();
        if (sel == null) return;

        try
        {
            _requestService.RejectRequest(sel.UserId, sel.AnimalId);
            LoadRequests();
        }
        catch (Exception ex)
        {
            var dlg = new Window { Width = 300, Height = 120, Content = new TextBlock { Text = ex.Message } };
            dlg.ShowDialog(this);
        }
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }
}
