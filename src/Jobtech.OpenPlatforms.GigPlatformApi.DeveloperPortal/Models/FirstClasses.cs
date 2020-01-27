using System;
using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models
{
    public class UserData
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Version DataVersion { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        private ICollection<string> documents;
        public ICollection<string> Documents
        {
            get { return documents ?? new List<string>(); }
            set { documents = value; }
        }
    }

    public class CVData
    {
        public string Id { get; set; }
        public Version DataVersion { get; set; }
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
        private ICollection<string> owners;
        public ICollection<string> Owners
        {
            get { return owners ?? new List<string>(); }
            set { owners = value; }
        }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        private CVDataCollection cVDataCollection;
        public CVDataCollection CVDataCollection { get { return cVDataCollection ?? new CVDataCollection(); } set { cVDataCollection = value; } }
    }

    public class CVDataCollection
    {
        public Version DataVersion { get; set; }
        private ICollection<GigPlatformUserData> gigWork;
        public ICollection<GigPlatformUserData> GigWork
        {
            get { return gigWork; }
            set { gigWork = value; }
        }

        //   profile: ProfileData = new ProfileData(); // our own data object
        // employments: PositionData[] = [];
        //   gigWork: GigPlatformData[] = []; // added
        //   projects: ProjectsData[] = [];// added
        //   educations: EducationData[] = [];
        //   courses: CourseData[] = [];
        //   certifications: CertificationData[] = [];
        //   publications: PublicationData[] = [];
        //   volunteer: VolunteerData[] = [];
        //   languages: LanguageData[] = [];
        //   patents: PatentData[] = [];
        //   skills: SkillData[] = [];
        //   recommendations: RecommendationData[] = [];
    }

    //     public class ProfileData
    //     {
    //         public Version DataVersion { get; set; }
    //   name: string = "";
    //   photo: string = "";
    //   birthdate: string = Utility.formatDate(new Date());
    //   location: LocationData = new LocationData();
    //         summary: string = "";
    //   interests: string = ""; // moved from top level
    //   associations: string = ""; // moved from top level
    //   links: LinkData[]=[];
    // }

    //     public class LinkData
    //     {
    //         public Version DataVersion { get; set; }
    //   title: string = "";
    //   url: string = "";
    // }


    //     public class LocationData
    //     {
    //         public Version DataVersion { get; set; }
    //   latitude: number = 0;
    //   longitude: number = 0;
    //   city: string = "";
    //   country: string = "";
    // }

    //     public class PositionData
    //     {
    //         public Version DataVersion { get; set; }
    //   public string Id { get; set; }
    //   title: string = "";
    //   summary: string = "";
    //   startDate: string = Utility.formatDate(new Date());
    //   endDate: string = Utility.formatDate(new Date());
    //   isOngoing: boolean = false;
    //   isCurrent: boolean = false;
    //   organization: OrganizationData = new OrganizationData();
    //     }

    public class GigPlatformUserData
    {
        // static normalizeData(data: GigPlatformData)
        // {
        //     if (data.averageRating == null)
        //     {
        //         data.averageRating = new RatingData();
        //     }
        // }

        public string UserId { get; set; }
        public Version DataVersion { get; set; }
    }

    public class RatingData
    {
        public Version DataVersion { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Value { get; set; }
    }

    public class ProjectsData
    {
        public string Id { get; set; }
        public Version DataVersion { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime CompletionDate { get; set; }
        private ICollection<ImageData> images;
        public ICollection<ImageData> Images
        {
            get { return images; }
            set { images = value; }
        }

    }

    public class ImageData
    {
        public string Id { get; set; }
        public Version DataVersion { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
    }

    // public class EducationData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   organization: OrganizationData = new OrganizationData();
    //     fieldOfStudy: string = "";
    //   startDate: string = Utility.formatDate(new Date());
    //   endDate: string = Utility.formatDate(new Date());
    //   isOngoing: boolean = false;
    //   degree: DegreeType = DegreeType.None;
    //   activities: string = ""; // unknown what this is supposed to be
    //   summary: string = "";
    // }

    // public class DegreeType
    // {
    //     static None: DegreeType = <DegreeType><any>"None";
    //   static SingleCourses: DegreeType = <DegreeType><any>"SingleCourses";
    //   static HigherEducationDiploma: DegreeType = <DegreeType><any>"HigherEducationDiploma";
    //   static Bachelor: DegreeType = <DegreeType><any>"Bachelor";
    //   static Master: DegreeType = <DegreeType><any>"Master";
    //   static Licenciate: DegreeType = <DegreeType><any>"Licenciate";
    //   static Doctorate: DegreeType = <DegreeType><any>"Doctorate";
    // }

    // public class CourseData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   name: string = "";
    //   organization: OrganizationData = new OrganizationData();
    //     startDate: string = Utility.formatDate(new Date());
    //   endDate: string = Utility.formatDate(new Date());
    //   score: number = 0; // added
    // }

    // public class CertificationData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   name: string = "";
    //   organization: OrganizationData = new OrganizationData();
    //     number: number = 0;
    //   startDate: string = Utility.formatDate(new Date());
    //   endDate: string = Utility.formatDate(new Date());
    //   score: number = 0; // added
    // }

    // public class LanguageData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   language: string = "";
    //   proficiency: LanguageProficiency;
    // }

    // public enum LanguageProficiency
    // {
    //     elementary, limitedWorking, professionalWorking, fullProfessional, nativeOrBilingual
    // }

    // public class RecommendationData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   relatedDataType: CVDataType;
    //   relatedData: string = ""; // id of related data
    //   recommender: string = ""; // Individual-id
    //   summary: string = "";
    //   rating?: RatingData;
    // }
    // public enum CVDataType
    // {
    //     IndividualData, PositionsData, IndependentWorkData, ProjectsData, EducationData, CourseData,
    //     CertificationData, PublicationData, VolunteerData, LanguageData, PatentData, SkillData
    // }

    // public class SkillData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   name: string = "";
    //   proficency: number = 0; // added
    // }

    // public class PublicationData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   title: string = "";
    //   publisher: OrganizationData = new OrganizationData();
    //     authors: IndividualData[] = [];
    //   date: string = Utility.formatDate(new Date());
    //   url: string = "";
    //   summary: string = "";
    // }

    // public class PatentData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   title: string = "";
    //   summary: string = "";
    //   number: number = 0;
    //   status: PatentStatus;
    //   office: OrganizationData = new OrganizationData();
    //     inventors: IndividualData[] = [];
    //   date: string = Utility.formatDate(new Date());
    //   url: string = "";
    // }

    // public enum PatentStatus
    // {
    //     Application, Patent
    // }


    // public class VolunteerData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   organization: OrganizationData = new OrganizationData();
    //     role: string = "";
    //   cause: string = "";
    //   opportunities: string = "";

    // }

    // public class OrganizationData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   name: string = "";
    //   country = "Sweden"
    //   type: OrganizationType;
    //   logo?: string = "";
    //   industry?: number = 0; // https://developer.linkedin.com/docs/reference/industry-codes
    //   ticker?: string = "";
    // }

    // public enum OrganizationType
    // {
    //     PublicCompany, PrivateCompany, University, GovermentOrganization, Other
    // }

    // public class IndividualData
    // {
    //     public string Id { get; set; }
    //   public Version DataVersion { get; set; }
    //   name: string = "";
    //   photo: string = "";
    // }
}
