Imports Microsoft.VisualBasic

Public Class FF_UserPreferences

    Private m_iShowCompletedJobs As Boolean
    Private m_iShowRecentlyCompletedJobs As Boolean
    Private m_iShowUnCompletedJobs As Boolean
    Private m_iShowTemplateJobs As Boolean
    Private m_iShowCancelledJobs As Boolean
    Private m_independentSearch As Boolean
    'Private Shared m_instance As FF_UserPreferences

    Public Property pShowCompletedJobs() As Boolean
        Get
            Return m_iShowCompletedJobs
        End Get
        Set(ByVal value As Boolean)
            m_iShowCompletedJobs = value
        End Set
    End Property

    Public Property pIndependentSearch() As Boolean
        Get
            Return m_independentSearch
        End Get
        Set(ByVal value As Boolean)
            m_independentSearch = value
        End Set
    End Property
    Public Property pShowTemplateJobs() As Boolean
        Get
            Return m_iShowTemplateJobs
        End Get
        Set(ByVal value As Boolean)
            m_iShowTemplateJobs = value
        End Set
    End Property
    Public Property pShowCancelledJobs() As Boolean
        Get
            Return m_iShowCancelledJobs
        End Get
        Set(ByVal value As Boolean)
            m_iShowCancelledJobs = value
        End Set
    End Property
    Public Property pShowRecentlyCompletedJobs() As Boolean
        Get
            Return m_iShowRecentlyCompletedJobs
        End Get
        Set(ByVal value As Boolean)
            m_iShowRecentlyCompletedJobs = value
        End Set
    End Property

    Public Property pShowUnCompletedJobs() As Boolean
        Get
            Return m_iShowUnCompletedJobs
        End Get
        Set(ByVal value As Boolean)
            m_iShowUnCompletedJobs = value
        End Set
    End Property


    'Public Shared Function GetInstance() As FF_UserPreferences
    '    If m_instance Is Nothing Then
    '        m_instance = New FF_UserPreferences()
    '    End If
    '    Return m_instance
    'End Function

End Class
