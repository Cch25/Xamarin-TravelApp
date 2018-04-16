using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RentFlixApp.Annotations;
using RentFlixApp.Extensions;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;

namespace RentFlixApp.ViewModels
{
    public class CardDataViewModel : INotifyPropertyChanged
    {
        private readonly StoryServices _storyServices = new StoryServices();
        private IList<CardDataModel> _cardDataCollection;
        private bool _isBusy;
        private bool _isSearchVisible;
        private IList<CardDataModel> _arrangedDataCollection;

        public IList<CardDataModel> ArrangedDataCollection
        {
            get => _arrangedDataCollection;
            set { _arrangedDataCollection = value; OnPropertyChanged(); }
        }

        public bool IsSearchVisible
        {
            get => _isSearchVisible;
            set { _isSearchVisible = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }


        public IList<CardDataModel> CardDataCollection
        {
            get => _cardDataCollection;
            set
            {
                _cardDataCollection = value;
                OnPropertyChanged();
            }
        }

        public object SelectedItem { get; set; }

        public CardDataViewModel()
        {
            GenerateCardModel();
        }

        public async void GenerateCardModel()
        {
            IsBusy = true;
            var story = await _storyServices.GetStoriesAsync(Settings.AccessToken);
            CardDataCollection = new ObservableCollection<CardDataModel>();
            AddToCollection(CardDataCollection, story.Stories, SortCollection.Descending);
            IsBusy = false;
        }

        public async void SearchCardViewModels(string keyword)
        {
            IsBusy = true;
            var stories = await _storyServices.SearchStoriesAsync(Settings.AccessToken, keyword);
            CardDataCollection = new ObservableCollection<CardDataModel>();
            AddToCollection(CardDataCollection, stories.Stories, SortCollection.Descending);
            IsBusy = false;
        }

        public async void GenerateArrangedDataCollection(int skip, int take)
        {
            IsBusy = true;
            var stories = await _storyServices.MostAppreciatedStories(Settings.AccessToken, skip, take);
            ArrangedDataCollection = new ObservableCollection<CardDataModel>();
            AddToCollection(ArrangedDataCollection, stories.Stories, SortCollection.Ascending);
            IsBusy = false;
        }

        private void AddToCollection(IList<CardDataModel> collection, IEnumerable<Story> story, SortCollection sortBy)
        {
            switch (sortBy)
            {
                case SortCollection.Ascending:
                    foreach (var s in story)
                    {
                        collection.Add(AddCardViewModel(s));
                    }
                    break;
                case SortCollection.Descending:
                    foreach (var s in story)
                    {
                        collection.Insert(0, AddCardViewModel(s));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null);
            }
        }

        private CardDataModel AddCardViewModel(Story story)
        {
            return new CardDataModel
            {
                PostId = story.Id,
                HeadTitle = story.HeadTitle,
                HeadLines = story.HeadLines.SafeSubstring(0, 130) + "...read more",
                HeadLinesDesc = story.HeadLines,
                BannerImage = Constants.BaseUrlNoSlash + story.BannerImage,
                ProfileImage = Constants.BaseUrlNoSlash + story.UserProfileModel.Avatar,
                DatePosted = ToHumanDate(story.CreateDate),
                ProfileName = story.UserProfileModel.FullName,
                TotalPictures = story.TotalPictures, //changed from rating to photos
                TotalComments = story.TotalComments,
                TotalLikes = story.TotalLikes
            };

        }

        private enum SortCollection
        {
            Ascending, Descending
        }
        public static string ToHumanDate(DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.Days >= 365)
            {
                var years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return $"{years} {(years == 1 ? "year" : "years")} ago";
            }

            if (span.Days > 30)
            {
                var months = span.Days / 30;
                if (span.Days % 31 != 0)
                    months += 1;
                return $"{months} {(months == 1 ? "month" : "months")} ago";
            }

            if (span.Days > 0)
                return $"{span.Days} {(span.Days == 1 ? "day" : "days")} ago";
            if (span.Hours > 0)
                return $"{span.Hours} {(span.Hours == 1 ? "hour" : "hours")} ago";
            if (span.Minutes > 0)
                return $"{span.Minutes} {(span.Minutes == 1 ? "minute" : "minutes")} ago";
            if (span.Seconds > 5)
                return $"{span.Seconds} seconds ago";
            return span.Seconds <= 5 ? "just now" : string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

