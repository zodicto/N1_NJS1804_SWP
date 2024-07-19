export interface complaintType {
    user: {
        id: string;
        fullName: string;
        email: string;
        date_of_birth: string;
        gender: string;
        avatar: string | null;
        address: string | null;
        phone: string;
        roles: string;
    };
    description: string;
    tutor: {
        id: string;
        fullName: string;
        email: string;
        date_of_birth: string;
        gender: string;
        avatar: string | null;
        address: string | null;
        phone: string;
        roles: string;
    };
}