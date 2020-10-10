using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

using TimeTable.Utility;
using TimeTable.Storage;
using TimeTable.Entity;
using TimeTable.ViewModel;
using System.Collections.Generic;

namespace TimeTable
{
    public partial class MainWindow : Window
    {
        private Database database = new SqliteDatabase(new DatabaseConfiguration { ConnectionString = "Data source=timetable.db" });

        private DefaultViewModel<LessonTime> lessonTimeViewModel;
        private DefaultViewModel<StudyGroup> studyGroupViewModel;
        private DefaultViewModel<Lesson> lessonViewModel;
        private DefaultViewModel<Classroom> classroomViewModel;
        private DefaultViewModel<Teacher> teacherViewModel;
        private SensitiveViewModel<Schedule> scheduleViewModel;
        private SensitiveViewModel<Change> changeViewModel;

        private string textFilter;

        private bool isScheduleEditPanelVisible = false;
        private bool isScheduleAddButtonVisible = true;

        private bool isChangeEditPanelVisible = false;
        private bool isChangeAddButtonVisible = true;

        private uint scheduleId;
        private uint changeId;

        private Dictionary<string, bool> scheduleExpanderStates = new Dictionary<string, bool>();
        private Dictionary<string, bool> changeExpanderStates = new Dictionary<string, bool>();


        public MainWindow()
        {
            InitializeComponent();

            lessonTimeViewModel = new LessonTimeViewModel(database);
            studyGroupViewModel = new StudyGroupViewModel(database);
            lessonViewModel = new LessonViewModel(database);
            classroomViewModel = new ClassroomViewModel(database);
            teacherViewModel = new TeacherViewModel(database);
            scheduleViewModel = new ScheduleViewModel(database);
            changeViewModel = new ChangeViewModel(database);

            lvSchedule.DataContext = scheduleViewModel;
            cbsLessonTime.DataContext = lessonTimeViewModel;
            cbsStudyGroup.DataContext = studyGroupViewModel;
            cbsLesson.DataContext = lessonViewModel;
            cbsClassroom.DataContext = classroomViewModel;
            cbsTeacher.DataContext = teacherViewModel;

            lvChanges.DataContext = changeViewModel;
            cbcLessonTime.DataContext = lessonTimeViewModel;
            cbcStudyGroup.DataContext = studyGroupViewModel;
            cbcLesson.DataContext = lessonViewModel;
            cbcClassroom.DataContext = classroomViewModel;
            cbcTeacher.DataContext = teacherViewModel;

            SettingsDescriptions();
        }

        private void SettingsDescriptions()
        {
            DescriptSetter.AddSort(lessonTimeViewModel.Model, "Start", "End");

            DescriptSetter.AddSort(studyGroupViewModel.Model, "Name");

            DescriptSetter.AddSort(lessonViewModel.Model, "Name");

            DescriptSetter.AddSort(classroomViewModel.Model, "Name");

            DescriptSetter.AddSort(teacherViewModel.Model, "Name");

            DescriptSetter.AddGroup(scheduleViewModel.Model, "StudyGroup.Name", "DayOfWeek");
            DescriptSetter.AddSort(scheduleViewModel.Model, "StudyGroup.Name", "DayOfWeek", "LessonTime.Start", "LessonTime.End");
            DescriptSetter.AddFilter(scheduleViewModel.Model, Filter);

            DescriptSetter.AddGroup(changeViewModel.Model, "StudyGroup.Name", "Date");
            DescriptSetter.AddSort(changeViewModel.Model, "StudyGroup.Name", "Date", "LessonTime.Start", "LessonTime.End");
            DescriptSetter.AddFilter(changeViewModel.Model, Filter);
        }

        // Filters
        private bool Filter(object item)
        {
            var obj = item as ICategoryFilter;

            if (string.IsNullOrEmpty(textFilter))
                return true;
            else
                return StudyGroupFilter(obj) || TeacherFilter(obj) || ClassroomFilter(obj);
        }

        private bool StudyGroupFilter(ICategoryFilter item)
        {
            return item.StudyGroupName.IndexOf(textFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool TeacherFilter(ICategoryFilter item)
        {
            return item.TeacherName.IndexOf(textFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool ClassroomFilter(ICategoryFilter item)
        {
            return item.ClassroomName.IndexOf(textFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }


        // Schedule tab control
        private void SwitchScheduleEditPanelVisible()
        {
            if (isScheduleEditPanelVisible)
                scheduleEditPanel.Visibility = Visibility.Collapsed;
            else
                scheduleEditPanel.Visibility = Visibility.Visible;

            isScheduleEditPanelVisible = !isScheduleEditPanelVisible;
        }

        private void SwitchScheduleButtonVisible()
        {
            if (isScheduleAddButtonVisible)
            {
                bScheduleAdd.Visibility = Visibility.Hidden;
                bScheduleEdit.Visibility = Visibility.Hidden;

                bScheduleEditConfirm.Visibility = Visibility.Visible;
                bScheduleEditCancel.Visibility = Visibility.Visible;
            }
            else
            {
                bScheduleEditConfirm.Visibility = Visibility.Hidden;
                bScheduleEditCancel.Visibility = Visibility.Hidden;

                bScheduleAdd.Visibility = Visibility.Visible;
                bScheduleEdit.Visibility = Visibility.Visible;
            }

            isScheduleAddButtonVisible = !isScheduleAddButtonVisible;
        }

        private bool ScheduleDataValidation()
        {
            // LessonTime check
            LessonTime _;
            cbsLessonTime.Text = cbsLessonTime.Text.Trim();
            if (cbsLessonTime.SelectedItem is null && !LessonTime.TryParse(cbsLessonTime.Text, out _))
            {
                MessageBox.Show("Неверный формат времени занятия");
                return false;
            }

            // StudyGroup check
            cbsStudyGroup.Text = cbsStudyGroup.Text.Trim();
            if (cbsStudyGroup.SelectedItem is null && string.IsNullOrEmpty(cbsStudyGroup.Text))
            {
                MessageBox.Show("Поле 'Учебная группа' не должно быть пустым");
                return false;
            }

            // Lesson check
            cbsLesson.Text = cbsLesson.Text.Trim();
            if (cbsLesson.SelectedItem is null && string.IsNullOrEmpty(cbsLesson.Text))
            {
                MessageBox.Show("Поле 'Дисциплина' не должно быть пустым");
                return false;
            }

            // Classroom check
            cbsClassroom.Text = cbsClassroom.Text.Trim();
            if (cbsClassroom.SelectedItem is null && string.IsNullOrEmpty(cbsClassroom.Text))
            {
                MessageBox.Show("Поле 'Аудитория' не должно быть пустым");
                return false;
            }

            // Teacher check
            cbsTeacher.Text = cbsTeacher.Text.Trim();
            if (cbsTeacher.SelectedItem is null && string.IsNullOrEmpty(cbsTeacher.Text))
            {
                MessageBox.Show("Поле 'Преподаватель' не должго быть пустым");
                return false;
            }

            return true;
        }

        private void ScheduleClearBoxes()
        {
            cbsLessonTime.Text = string.Empty;
            cbsLesson.Text = string.Empty;
            cbsClassroom.Text = string.Empty;
            cbsTeacher.Text = string.Empty;
            tbsInfo.Text = string.Empty;
        }

        private void ScheduleExpanderStateSave(Expander expander)
        {
            var cvg = expander.DataContext as CollectionViewGroup;
            var groupName = cvg.Name.ToString();

            scheduleExpanderStates[groupName] = expander.IsExpanded;
        }

        private Schedule ScheduleDataCollect()
        {
            var dayOfWeek = (DayOfWeek)cbsDayOfWeek.SelectedItem;

            var lessonTime = cbsLessonTime.SelectedItem as LessonTime;
            if (lessonTime is null)
            {
                lessonTime = LessonTime.Parse(cbsLessonTime.Text);
                lessonTimeViewModel.Add(lessonTime);
            }

            var studyGroup = cbsStudyGroup.SelectedItem as StudyGroup;
            if (studyGroup is null)
            {
                studyGroup = new StudyGroup { Name = cbsStudyGroup.Text };
                studyGroupViewModel.Add(studyGroup);
                cbsStudyGroup.SelectedValue = studyGroup.Id;
            }


            var lesson = cbsLesson.SelectedItem as Lesson;
            if (lesson is null)
            {
                lesson = new Lesson { Name = cbsLesson.Text };
                lessonViewModel.Add(lesson);
            }

            var classroom = cbsClassroom.SelectedItem as Classroom;
            if (classroom is null)
            {
                classroom = new Classroom { Name = cbsClassroom.Text };
                classroomViewModel.Add(classroom);
            }

            var teacher = cbsTeacher.SelectedItem as Teacher;
            if (teacher is null)
            {
                teacher = new Teacher { Name = cbsTeacher.Text };
                teacherViewModel.Add(teacher);
            }

            var additionalInfo = tbsInfo.Text;

            return new Schedule
            {
                DayOfWeek = dayOfWeek,
                LessonTime = lessonTime,
                StudyGroup = studyGroup,
                Lesson = lesson,
                Classroom = classroom,
                Teacher = teacher,
                AdditionalInfo = additionalInfo
            };
        }


        private void scheduleExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            ScheduleExpanderStateSave(sender as Expander);
        }

        private void scheduleExpander_Expanded(object sender, RoutedEventArgs e)
        {
            ScheduleExpanderStateSave(sender as Expander);
        }

        private void scheduleExpander_Loaded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            var cvg = expander.DataContext as CollectionViewGroup;

            var groupName = cvg.Name.ToString();

            if (scheduleExpanderStates.ContainsKey(groupName))
                expander.IsExpanded = scheduleExpanderStates[groupName];
            else
                scheduleExpanderStates[groupName] = expander.IsExpanded;
        }

        private void tbScheduleFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textFilter = tbScheduleFilter.Text;
            CollectionViewSource.GetDefaultView(scheduleViewModel.Model).Refresh();
        }

        
        private void bScheduleAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleDataValidation())
            {
                var schedule = ScheduleDataCollect();

                scheduleViewModel.Add(schedule);

                ScheduleClearBoxes();
                cbsLessonTime.Focus();
            }
        }

        private void bScheduleEditConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleDataValidation())
            {
                var schedule = ScheduleDataCollect();
                schedule.Id = scheduleId;

                if (!scheduleViewModel.Update(schedule))
                {
                    MessageBox.Show("Не удалось изменить расписание.");
                    return;
                }

                SwitchScheduleButtonVisible();
                SwitchScheduleEditPanelVisible();
                ScheduleClearBoxes();
            }
        }

        private void scheduleItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var schedule = (sender as MenuItem).DataContext as Schedule;

            var result = MessageBox.Show($"Удалить расписание №{schedule.Id}", "Сообщение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (!scheduleViewModel.Remove(schedule))
                {
                    MessageBox.Show($"Не удалось удалить расписание №{schedule.Id}");
                }
            }
        }


        private void bScheduleEdit_Click(object sender, RoutedEventArgs e)
        {
            SwitchScheduleEditPanelVisible();
        }

        private void scheduleItemEdit_Click(object sender, RoutedEventArgs e)
        {
            SwitchScheduleButtonVisible();

            if (!isScheduleEditPanelVisible)
                SwitchScheduleEditPanelVisible();

            var schedule = (sender as MenuItem).DataContext as Schedule;

            scheduleId = schedule.Id;
            cbsDayOfWeek.SelectedValue = schedule.DayOfWeek;
            cbsLessonTime.SelectedValue = schedule.LessonTime.Id;
            cbsStudyGroup.SelectedValue = schedule.StudyGroup.Id;
            cbsLesson.SelectedValue = schedule.Lesson.Id;
            cbsClassroom.SelectedValue = schedule.Classroom.Id;
            cbsTeacher.SelectedValue = schedule.Teacher.Id;
            tbsInfo.Text = schedule.AdditionalInfo;
        }

        private void bScheduleEditCancel_Click(object sender, RoutedEventArgs e)
        {
            ScheduleClearBoxes();
            SwitchScheduleButtonVisible();
            SwitchScheduleEditPanelVisible();
        }

        
        // Changes tab control
        private void SwitchChangeEditPanelVisible()
        {
            if (isChangeEditPanelVisible)
                changeEditPanel.Visibility = Visibility.Collapsed;
            else
                changeEditPanel.Visibility = Visibility.Visible;

            isChangeEditPanelVisible = !isChangeEditPanelVisible;
        }

        private void SwitchChangeButtonVisible()
        {
            if (isChangeAddButtonVisible)
            {
                bChangeAdd.Visibility = Visibility.Hidden;
                bChangeEdit.Visibility = Visibility.Hidden;

                bChangeEditConfirm.Visibility = Visibility.Visible;
                bChangeEditCancel.Visibility = Visibility.Visible;
            }
            else
            {
                bChangeEditConfirm.Visibility = Visibility.Hidden;
                bChangeEditCancel.Visibility = Visibility.Hidden;

                bChangeAdd.Visibility = Visibility.Visible;
                bChangeEdit.Visibility = Visibility.Visible;
            }

            isChangeAddButtonVisible = !isChangeAddButtonVisible;
        }

        private bool ChangeDataValidation()
        {
            // Date check
            if (datePicker.SelectedDate is null)
            {
                MessageBox.Show("Поле 'Дата' не должно быть пустым");
                return false;
            }

            // LessonTime check
            LessonTime _;
            cbcLessonTime.Text = cbcLessonTime.Text.Trim();
            if (cbcLessonTime.SelectedItem is null && !LessonTime.TryParse(cbcLessonTime.Text, out _))
            {
                MessageBox.Show("Неверный формат времени занятия");
                return false;
            }

            // StudyGroup check
            cbcStudyGroup.Text = cbcStudyGroup.Text.Trim();
            if (cbcStudyGroup.SelectedItem is null && string.IsNullOrEmpty(cbcStudyGroup.Text))
            {
                MessageBox.Show("Поле 'Учебная группа' не должно быть пустым");
                return false;
            }

            /// Lesson check
            cbcLesson.Text = cbcLesson.Text.Trim();
            if (cbcLesson.SelectedItem is null && string.IsNullOrEmpty(cbcLesson.Text))
            {
                MessageBox.Show("Поле 'Дисциплина' не должно быть пустым");
                return false;
            }

            // Classroom check
            cbcClassroom.Text = cbcClassroom.Text.Trim();
            if (cbcClassroom.SelectedItem is null && string.IsNullOrEmpty(cbcClassroom.Text))
            {
                MessageBox.Show("Поле 'Аудитория' не должно быть пустым");
                return false;
            }

            // Teacher check
            cbcTeacher.Text = cbcTeacher.Text.Trim();
            if (cbcTeacher.SelectedItem is null && string.IsNullOrEmpty(cbcTeacher.Text))
            {
                MessageBox.Show("Поле 'Преподаватель' не должго быть пустым");
                return false;
            }

            return true;
        }

        private void ChangeClearBoxes()
        {
            cbcLessonTime.Text = string.Empty;
            cbcStudyGroup.Text = string.Empty;
            cbcLesson.Text = string.Empty;
            cbcClassroom.Text = string.Empty;
            cbcTeacher.Text = string.Empty;
            tbcInfo.Text = string.Empty;
        }

        private void ChangeExpanderStatesSave(Expander expander)
        {
            var cvg = expander.DataContext as CollectionViewGroup;
            var groupName = cvg.Name.ToString();

            changeExpanderStates[groupName] = expander.IsExpanded;
        }
        
        private Change ChangeDataCollect()
        {
            var date = datePicker.SelectedDate.Value;

            var lessonTime = cbcLessonTime.SelectedItem as LessonTime;
            if (lessonTime is null)
            {
                lessonTime = LessonTime.Parse(cbcLessonTime.Text);
                lessonTimeViewModel.Add(lessonTime);
            }


            var studyGroup = cbcStudyGroup.SelectedItem as StudyGroup;
            if (studyGroup is null)
            {
                studyGroup = new StudyGroup { Name = cbcStudyGroup.Text };
                studyGroupViewModel.Add(studyGroup);
                cbcStudyGroup.SelectedValue = studyGroup.Id;
            }

            var lesson = cbcLesson.SelectedItem as Lesson;
            if (lesson is null)
            {
                lesson = new Lesson { Name = cbcLesson.Text };
                lessonViewModel.Add(lesson);
            }

            var classroom = cbcClassroom.SelectedItem as Classroom;
            if (classroom is null)
            {
                classroom = new Classroom { Name = cbcClassroom.Text };
                classroomViewModel.Add(classroom);
            }

            var teacher = cbcTeacher.SelectedItem as Teacher;
            if (teacher is null)
            {
                teacher = new Teacher { Name = cbcTeacher.Text };
                teacherViewModel.Add(teacher);
            }

            var additionalInfo = tbcInfo.Text;

            return new Change
            {
                Date = date,
                LessonTime = lessonTime,
                StudyGroup = studyGroup,
                Lesson = lesson,
                Classroom = classroom,
                Teacher = teacher,
                AdditionalInfo = additionalInfo
            };
        }


        private void changeExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            ChangeExpanderStatesSave(sender as Expander);
        }

        private void changeExpander_Expanded(object sender, RoutedEventArgs e)
        {
            ChangeExpanderStatesSave(sender as Expander);
        }

        private void changeExpander_Loaded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            var cvg = expander.DataContext as CollectionViewGroup;
            var groupName = cvg.Name.ToString();

            if (changeExpanderStates.ContainsKey(groupName))
                expander.IsExpanded = changeExpanderStates[groupName];
            else
                changeExpanderStates[groupName] = expander.IsExpanded;
        }

        private void tbChangeFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textFilter = tbChangeFilter.Text;
            CollectionViewSource.GetDefaultView(changeViewModel.Model).Refresh();
        }


        private void bChangeAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeDataValidation())
            {
                var change = ChangeDataCollect();

                changeViewModel.Add(change);

                ChangeClearBoxes();
                cbcLessonTime.Focus();
            }
        }

        private void bChangeEditConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeDataValidation())
            {
                var change = ChangeDataCollect();
                change.Id = changeId;

                if (!changeViewModel.Update(change))
                {
                    MessageBox.Show("Не удалось изменить замену.");
                    return;
                }

                SwitchChangeButtonVisible();
                SwitchChangeEditPanelVisible();
                ChangeClearBoxes();
            }
        }

        private void changeItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var change = (sender as MenuItem).DataContext as Change;

            var result = MessageBox.Show($"Удалить замену №{change.Id}", "Сообщение", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (!changeViewModel.Remove(change))
                {
                    MessageBox.Show($"Не удалось удалить замену №{change.Id}");
                }
            }
        }


        private void bChangeEdit_Click(object sender, RoutedEventArgs e)
        {
            SwitchChangeEditPanelVisible();
        }

        private void changeItemEdit_Click(object sender, RoutedEventArgs e)
        {
            SwitchChangeButtonVisible();

            if (!isChangeEditPanelVisible)
                SwitchChangeEditPanelVisible();

            var change = (sender as MenuItem).DataContext as Change;

            changeId = change.Id;
            datePicker.SelectedDate = change.Date;
            cbcLessonTime.SelectedValue = change.LessonTime.Id;
            cbcStudyGroup.SelectedValue = change.StudyGroup.Id;
            cbcLesson.SelectedValue = change.Lesson.Id;
            cbcClassroom.SelectedValue = change.Classroom.Id;
            cbcTeacher.SelectedValue = change.Teacher.Id;
            tbcInfo.Text = change.AdditionalInfo;
        }

        private void bChangeEditCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangeClearBoxes();
            SwitchChangeButtonVisible();
            SwitchChangeEditPanelVisible();
        }

    }
}
