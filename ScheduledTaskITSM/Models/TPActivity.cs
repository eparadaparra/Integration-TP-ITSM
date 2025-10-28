namespace ScheduledTaskITSM.Models
{
    public partial class TPActivity
    {
        public Data? data { get; set; }
        public string? message { get; set; }
        public string? status { get; set; }
        public string? statusCode { get; set; }
        public Mensaje? messageError { get; set; }
    }

    public partial class Mensaje
    {
        public string? code { get; set; }
        public string? message { get; set; }
    }
    public partial class Data
    {
        public string? status { get; set; }
        public string? scheduled_user_email { get; set; }    
        public string? latitude { get; set; }
        public string? longitude { get; set; }
        public string? firebase_id { get; set; }
        public string? scheduled_date_programming { get; set; }  //yyyy-mm-dd
        public string? scheduled_hour_since { get; set; }
        public string? scheduledInstructions { get; set; }
        public string? dynamic_url_pdf { get; set; }
        public string? dynamic_url_web { get; set; }
        public string? classification_category_name { get; set; }
        public string? classification_subcategory_name { get; set; }
        public string? duration { get; set; }
        public string? order_number { get; set; }


        public TimeZone time_zone { get; set; }
        public List<Preload>? preload { get; set; }
        public StatusInfo? statusInfo { get; set; }
        public DateSeparated? startDateSeparated { get; set; }
        public DateSeparated? endDateSeparated { get; set; }
        public List<Element>? elements { get; set; }
        public User? user { get; set; }

        //====================================================

        public bool active_out_checkout_range { get; set; }
        public string? address { get; set; }
        public string? address_checkIn { get; set; }
        public string? authorization_comment { get; set; }
        public object authorization_status { get; set; }
        public bool authorized_event { get; set; }
        public string? category { get; set; }
        public object classification_category_uid { get; set; }
        public object classification_subcategory_uid { get; set; }
        public string? client_id { get; set; }
        public long comentarios { get; set; }
        public List<object> contacts { get; set; }
        public SecondsNano createdAt { get; set; }
        public bool created_api { get; set; }
        public long customer { get; set; }
        public string? customer_name { get; set; }
        public bool dynamic_url_enter { get; set; }
        public string? end_date { get; set; }
        public object end_date_utc2 { get; set; }
        public Geolocation geolocation { get; set; }
        public bool is_out_checkout_range { get; set; }
        public bool is_reasigned { get; set; }
        public bool is_utc { get; set; }
        public List<string?> keywords { get; set; }
        public ModulesConfig modules_config { get; set; }
        public string? order_name { get; set; }
        public bool outtime { get; set; }
        public string? photo_user { get; set; }
        public string? qr_src { get; set; }
        public List<object> rows { get; set; }
        public bool scheduled { get; set; }
        public string? scheduled_address { get; set; }
        public object scheduled_assigned_to { get; set; }
        public object scheduled_clasification { get; set; }
        public object scheduled_clasification_name { get; set; }
        public string? scheduled_client_uuid { get; set; }
        public string? scheduled_colony { get; set; }
        public SecondsNano scheduled_date_creation { get; set; }
        public object scheduled_equipment_uid { get; set; }
        public string? scheduled_estimated_duration { get; set; }
        public long scheduled_estimated_duration_hours { get; set; }
        public long scheduled_estimated_duration_minutes { get; set; }
        public long scheduled_expiration_date { get; set; }
        public string? scheduled_hour_limit { get; set; }
        public string? scheduled_instructions { get; set; }
        public SecondsNano scheduled_limit_date { get; set; }
        //public double? scheduled_map_latitude { get; set; }
        //public double? scheduled_map_longitude { get; set; }
        public string? scheduled_mother_serie { get; set; }
        public string? scheduled_name_event { get; set; }
        public string? scheduled_num_ext { get; set; }
        public string? scheduled_num_int { get; set; }
        public string? scheduled_periodicity { get; set; }
        public string? scheduled_postal_address { get; set; }
        public SecondsNano scheduled_programming_date { get; set; }
        public string? scheduled_state { get; set; }
        public string? scheduled_street { get; set; }
        public object scheduled_subclasification { get; set; }
        public object scheduled_subclasification_name { get; set; }
        public string? scheduled_town { get; set; }
        public string? scheduled_type_event { get; set; }
        public object scheduled_user_uuid { get; set; }
        public List<object> scheduled_zone { get; set; }
        public Settings settings { get; set; }
        public string? start_date { get; set; }
        public SecondsNano start_date_utc { get; set; }
        public string? start_date_utc2 { get; set; }
        public string? status_reject_assigned { get; set; }
        public string? title_category { get; set; }
        public string? title_subcategory { get; set; }
        public object tmp_scheduled_assigned_to { get; set; }
        public object tmp_user_id { get; set; }
        public string? url_map { get; set; }
        public string? user_created { get; set; }
        public string? user_name { get; set; }
        public object user_uid { get; set; }
        public List<object> users { get; set; }
        public bool users_all { get; set; }
        public List<User> users_all_array { get; set; }
    }
    public partial class DateSeparated
    {
        public string? date { get; set; }
        public string? time { get; set; }
    }
    public partial class SecondsNano
    {
        public long _nanoseconds { get; set; }
        public long _seconds { get; set; }
    }
    public partial class Preload
    {
        public string? frmRecIdTask { get; set; }
        public string? frmAssignmentId { get; set; }
        public string? frmParentNumber { get; set; }
        public string? frmParentCategory { get; set; }
        public string? frmIdSitio { get; set; }
        public string? frmCustId { get; set; }
        public string? frmCodigoCierre { get; set; }
        public string? frmParentOwner { get; set; }
        public string? frmServer { get; set; }
        //public string? frmIdZona { get; set; }
    }
    public partial class StatusInfo
    {
        public string? color { get; set; }
        public string? txt { get; set; }
    }
    public partial class Element
    {
        public string? title { get; set; }
        public string? type { get; set; }
        public bool? isMap { get; set; }
        public string? urlMap { get; set; }
        public bool? isPhotos { get; set; }
        public bool? isQuestionnaires { get; set; }
        public bool? isSignature { get; set; }
        public User? user { get; set; }

        public Info? info { get; set; }
        public List<Items>? items { get; set; }
    }
    public partial class User
    {
        public string? name { get; set; }
        public string? photo_Url { get; set; }
        public string? uid { get; set; }
        public string? email { get; set; }
    }
    public partial class Info
    {
        public string? address { get; set; }
        public string? checkDate { get; set; }
        public Geolocation? geolocation { get; set; }
        public string? latitude { get; set; }
        public string? longitude { get; set; }
    }
    public partial class Geolocation
    {
        public string? geohash { get; set; }
        public Geopoint? geopoint { get; set; }
    }
    public partial class Geopoint
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }

    public partial class Items
    {
        public string? title { get; set; }
        public string? type { get; set; }
        public bool thumbnail { get; set; }
        public string? thumbnail_url { get; set; }
        public bool? isPhoto { get; set; }
        public bool? isInstruction { get; set; }
        public bool? isNormal { get; set; }
        //public string? value { get; set; }

    }
    public partial class TimeZone
    {
        public string? abbreviation { get; set; }
        public string? firebase_id { get; set; }
        public string? label { get; set; }
        public string? label_short { get; set; }
        public string? name { get; set; }
        public string? reference { get; set; }
    }
    public partial class ModulesConfig
    {
        public bool active { get; set; }
        public bool active_autorize_events { get; set; }
        public bool active_carret { get; set; }
        public bool active_category { get; set; }
        public bool active_out_range { get; set; }
        public bool active_transfer { get; set; }
        public string? category { get; set; }
        public string? collectionSearch { get; set; }
        public bool createElements { get; set; }
        public bool createOrders { get; set; }
        public long date_created { get; set; }
        public List<string?> forms { get; set; }
        public string? name { get; set; }
        public string? order { get; set; }
        public bool reject_assignment { get; set; }
        public bool relationOneToMany { get; set; }
        public bool selectCustomer { get; set; }
        public bool selectEquipment { get; set; }
        public UsersAutorized users_autorized { get; set; }
    }
    public partial class UsersAutorized
    {
        public bool all { get; set; }
        public List<string?> user_uid { get; set; }
    }
    public partial class Settings
    {
        public bool cloneOrders { get; set; }
        public string? color_pdf { get; set; }
        public string? comentarios { get; set; }
        public bool createItems { get; set; }
        public bool createOrders { get; set; }
        public string? customer_team_name { get; set; }
        public bool deleteItems { get; set; }
        public bool deleteOrders { get; set; }
        public bool descriptionEvidenceRequired { get; set; }
        public string? elastic_authorization { get; set; }
        public string? elastic_endpoint_search { get; set; }
        public string? elastic_url { get; set; }
        public string? icon_comentarios { get; set; }
        public string? icon_photo { get; set; }
        public string? icon_signature { get; set; }
        public string? label_name { get; set; }
        public string? logo_client { get; set; }
        public long maxNumberPhotos { get; set; }
        public bool relationOneToMany { get; set; }
        public string? sendgrid_api_key { get; set; }
        public string? sendgrid_email { get; set; }
        public string? sendgrid_email_name { get; set; }
        public string? sendgrid_forgot_password_subject { get; set; }
        public string? sendgrid_forgot_password_template_id { get; set; }
        public string? sendgrid_notification_subject { get; set; }
        public string? sendgrid_notification_url { get; set; }
        public string? sendgrid_notification_url_site { get; set; }
        public string? sendgrid_subject { get; set; }
        public string? sendgrid_template_id { get; set; }
        public string? sendgrid_template_notification_finalized_id { get; set; }
        public string? sendgrid_template_notification_ics_id { get; set; }
        public string? sendgrid_template_notification_id { get; set; }
        public string? sendgrid_template_notification_scheduled_customer_id { get; set; }
        public string? sendgrid_template_notification_scheduled_id { get; set; }
        public string? sendgrid_template_reject_id { get; set; }
        public string? sendgrid_url_path { get; set; }
        public string? sengrid_url_pdf { get; set; }
        public string? storage { get; set; }
        public string? timezone_id { get; set; }
        public string? total_get_items_api { get; set; }
        public string? total_num_orders { get; set; }
        public string? url_num_orders { get; set; }
        public string? url_pdf_web { get; set; }
        public string? url_pdf_web_simply { get; set; }
        public string? url_report { get; set; }
        public string? webhook_father_header { get; set; }
        public string? webhook_father_url { get; set; }
        public string? webhook_header { get; set; }
        public string? webhook_notify_email { get; set; }
        public string? webhook_notify_header { get; set; }
        public string? webhook_notify_url { get; set; }
        public string? webhook_url { get; set; }
    }

}
