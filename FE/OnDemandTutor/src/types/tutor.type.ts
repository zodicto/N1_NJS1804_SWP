export interface TutorType {
  id: string
  fullName: string
  date_of_birth: string
  gender: string
  avatar: string
  specializedSkills: string
  experience: number
  subject: string
  qualifiCationName: string
  type: string
  imageQualification: string
  introduction: string
  rating: number
}

export interface UpdateTutorProfile {
  specializedSkill: string
  experience: number
  introduction: string
}

export interface AddQualification {
  name: string
  img: string
  type: string
}

export interface TutorProfile {
  avatar: string
  specializedSkills: string
  experience: number
  introduction: string
  subjects: string
  qualifications: {
    id: string
    name: string
    img: string
    type: string
  }[]
}

export interface CensorShipTutor {
  id: string
  specializedSkills: string
  introduction: string
  date_of_birth: string
  fullName: string
  gender: string
  experience: number
  subject: string
  qualifiCationName: string
  type: string
  imageQualification: string
  status: string
  reason: string
}

export interface AdminTutorType {
  id: string
  fullName: string
  date_of_birth: string
  gender: string
  specializedSkills: string
  experience: number
  subject: string
  qualifiCationName: string
  type: string
  imageQualification: string
  introduction: string
  avatar: string
  rating: number
}
interface AdminStudentReq {
  idrequest: string
  fullname: string
  subject: string
  title: string
  timetable: string
  price: number
  description: string
  class: string
  learningmethod: string
  date: string
  timestart: string
  timeend: string
}
export interface CreatServiceType {
  pricePerHour: number
  title: string
  subject: string
  class: string
  description: string
  learningMethod: string
  schedule: ScheduleType[]
}

interface ScheduleType {
  date: string
  timeSlots: (string | undefined)[]
}

export interface AdminTutorProfile {
  id: string
  avatar: string
  fullName: string
  date_of_birth: string
  gender: string
  specializedSkills: string
  experience: number
  subjects: string
  rating: number
  qualifications: [
    {
      idQualifications: string
      qualificationName: string
      img: string
      type: string
    }
  ]
  introduction: string
}
