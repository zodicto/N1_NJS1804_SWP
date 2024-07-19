import * as yup from 'yup'

function testTime(this: yup.TestContext<yup.AnyObject>) {
  const { timeStart, timeEnd } = this.parent as {
    timeStart: string
    timeEnd: string
  }
  if (timeStart && timeEnd) {
    const start = new Date(`1970-01-01T${timeStart}:00Z`) // Adding reference date
    const end = new Date(`1970-01-01T${timeEnd}:00Z`) // Adding reference date

    return start < end
  }
  return timeStart === '' || timeEnd === ''
}

const handleConfirmPasswordYup = (refString: string) => {
  return yup
    .string()
    .required('Mật khẩu là bắt buộc')
    .min(6, 'Độ dài từ 6-160 ký tự')
    .max(160, 'Độ dài từ 6-160 ký tự')
    .oneOf([yup.ref(refString)], 'Nhập lại mật khẩu không khớp ')
}

export const schema = yup.object({
  email: yup
    .string()
    .required('Email là bắt buộc')
    .min(5, 'Độ dài từ 5-160 ký tự')
    .max(160, 'Độ dài từ 5-160 ký tự'),
  password: yup
    .string()
    .required('Mật Khẩu là bắt buộc')
    .min(6, 'Độ dài từ 6-160 ký tự')
    .max(160, 'Độ dài từ 6-160 ký tự'),
  confirm_password: yup
    .string()
    .required('Xác nhận mật khẩu là bắt buộc')
    .min(6, 'Độ dài từ 6-160 ký tự')
    .max(160, 'Độ dài từ 6-160 ký tự')
    .oneOf([yup.ref('password')], 'Nhập lại mật khẩu không khớp'),

  phone: yup
    .string()
    .max(20, 'Độ dài tối đa là 20 ký tự')
    .required('Số điện thoại là bắt buộc'),
  avatar: yup.string().max(1000, 'Độ dài tối đa là 1000 ký tự'),
  address: yup.string().max(160, 'Độ dài tối đa là 160 ký tự'),
  date_of_birth: yup.date().max(new Date(), 'Hãy chọn một ngày trong quá khứ'),
  gender: yup.string().oneOf(['nam', 'nữ']),
  amount: yup.number().required('Số tiền nạp là bắt buộc'),

  fullName: yup
    .string()
    .max(160, 'Độ dài tối đa là 160 ký tự')
    .required('Họ và tên là bắt buộc')
})

export const userSchema = yup.object({
  email: yup
    .string()
    .required('Email là bắt buộc')
    .min(5, 'Độ dài từ 5-160 ký tự')
    .max(160, 'Độ dài từ 5-160 ký tự'),
  phone: yup.string().max(20, 'Độ dài tối đa là 20 ký tự'),
  avatar: yup.string().max(1000, 'Độ dài tối đa là 1000 ký tự'),
  address: yup.string().max(160, 'Độ dài tối đa là 160 ký tự'),
  date_of_birth: yup.date().max(new Date(), 'Hãy chọn một ngày trong quá khứ'),
  password: schema.fields['password'],
  new_password: schema.fields['password'],
  confirm_password: handleConfirmPasswordYup('new_password'),
  gender: yup.string().oneOf(['male', 'female']),
  fullName: yup
    .string()
    .max(160, 'Độ dài tối đa là 160 ký tự')
    .required('Họ và tên là bắt buộc')
})

export const updateSchema = yup.object({
  phone: yup.string().max(20, 'Độ dài tối đa là 20 ký tự'),
  avatar: yup.string().max(1000, 'Độ dài tối đa là 1000 ký tự'),
  address: yup.string().max(160, 'Độ dài tối đa là 160 ký tự'),
  date_of_birth: yup.date().max(new Date(), 'Hãy chọn một ngày trong quá khứ'),
  email: yup.string(),

  gender: yup.string().oneOf(['nam', 'nữ']),
  fullName: yup
    .string()
    .max(160, 'Độ dài tối đa là 160 ký tự')
    .required('Họ và tên là bắt buộc'),
  roles: yup.string()
})

export const requestSchema = yup.object({
  title: yup.string().required('Tựa đề là bắt buộc'),

  timeTable: yup.string().required('Chọn buổi học là bắt buộc'), // Expect an array of strings for timetable

  totalSessions: yup.number(),
  learningMethod: yup
    .string()
    .oneOf(
      ['Dạy trực tiếp(offline)', 'Dạy trực tuyến (online)'],
      'Phương thức học không hợp lệ'
    )
    .required('Hãy chọn phương thức học'),

  class: yup.string().oneOf(['10', '11', '12']).required('Chọn lớp'),
  price: yup
    .number()
    .min(30000, 'Mỗi buổi giá ít nhất là 30,000 VNĐ')
    .transform((value, originalValue) => {
      // Ensure originalValue is a string before calling replace
      if (typeof originalValue === 'string') {
        const formattedValue = parseFloat(originalValue.replace(/,/g, ''))
        return isNaN(formattedValue) ? 0 : formattedValue
      }
      return originalValue
    })
    .required('Giá là bắt buộc')
    .positive('Giá không thể là số âm'),
  subject: yup
    .string()
    .required('Môn học là bắt buộc')
    .oneOf(
      [
        'Ngữ văn',
        'Toán học',
        'Vật lý',
        'Hóa học',
        'Sinh học',
        'Lịch sử',
        'Địa lý',
        'Giáo dục công dân',
        'Ngoại ngữ',
        'Tin học'
      ],
      'Môn học không hợp lệ'
    ),
  timeEnd: yup.string().required('Thời gian kết thúc là bắt buộc').test({
    name: 'time-not-allowed',
    message: 'thời gian không phù hợp',
    test: testTime
  }),

  timeStart: yup.string().required('Thời gian bắt đầu là bắt buộc').test({
    name: 'time-not-allowed',
    message: 'thời gian không phù hợp',
    test: testTime
  }),
  description: yup
    .string()
    .required('Mô tả là bắt buộc')
    .max(1000, 'Độ dài tối đa 1000 ký tự')
})

export const schemaResAT = yup.object({
  introduction: yup.string().required('Giới thiệu bản thân là bắt buộc'),
  qualificationName: yup.string().required('Không được bỏ trống'),
  type: yup
    .string()
    .oneOf(['Chứng Chỉ', 'Bằng Cắp'], 'Phải là Chứng Chỉ hoặc bằng cấp'),

  subject: yup
    .string()
    .required('Môn học là bắt buộc')
    .oneOf(
      [
        'Ngữ văn',
        'Toán học',
        'Vật lý',
        'Hóa học',
        'Sinh học',
        'Lịch sử',
        'Địa lý',
        'Giáo dục công dân',
        'Ngoại ngữ',
        'Tin học'
      ],
      'Môn học không hợp lệ'
    ),
  experience: yup
    .number()
    .required('Số năm kinh nghiệm là bắt buộc')
    .min(0, 'Số năm kinh nghiệm phải lớn hơn 0'),
  specializedSkills: yup.string().required('Kỹ năng chuyên môn là bắt buộc'),
  imageQualification: yup.string().required('Ảnh còn thiếu')
})

export const updateTT = yup.object({
  introduction: yup.string().required('Giới thiệu bản thân là bắt buộc'),
  qualificationName: yup.string().required('Tên bằng cấp là bắt buộc'),
  type: yup
    .string()
    .required('Loại là bắt buộc')
    .oneOf(['Chứng Chỉ', 'Bằng Cấp'], 'Loại phải là Chứng Chỉ hoặc Bằng Cấp'),

  subjects: yup
    .string()
    .required('Môn học là bắt buộc')
    .oneOf(
      [
        'Ngữ văn',
        'Toán học',
        'Vật lý',
        'Hóa học',
        'Sinh học',
        'Lịch sử',
        'Địa lý',
        'Giáo dục công dân',
        'Ngoại ngữ',
        'Tin học'
      ],
      'Môn học không hợp lệ'
    ),
  experience: yup
    .number()
    .required('Số năm kinh nghiệm là bắt buộc')
    .min(1, 'Số năm kinh nghiệm phải lớn hơn 0'),
  speacializedSkill: yup.string().required('Kỹ năng chuyên môn là bắt buộc'),
  qualifications: yup.object({
    id: yup.string().required('ID là bắt buộc'),
    name: yup.string().required('Tên bằng cấp là bắt buộc'),
    img: yup.string().required('Ảnh chứng chỉ là bắt buộc'),
    type: yup.string().required('Loại là bắt buộc')
  })
})

export const updateProfileTT = yup.object({
  introduction: yup.string().required('Giới thiệu bản thân là bắt buộc'),

  experience: yup
    .number()
    .required('Số năm kinh nghiệm là bắt buộc')
    .min(1, 'Số năm kinh nghiệm phải lớn hơn 0'),
  specializedSkill: yup.string().required('Kỹ năng chuyên môn là bắt buộc')
})

export const updateMajorTT = yup.object({
  subjects: yup
    .string()
    .required('Môn học là bắt buộc')
    .oneOf(
      [
        'Ngữ văn',
        'Toán học',
        'Vật lý',
        'Hóa học',
        'Sinh học',
        'Lịch sử',
        'Địa lý',
        'Giáo dục công dân',
        'Ngoại ngữ',
        'Tin học'
      ],
      'Môn học không hợp lệ'
    ),
  name: yup.string().required('Tên bằng cấp là bắt buộc'),
  img: yup.string().required('Ảnh chứng chỉ là bắt buộc'),
  type: yup.string().required('Loại là bắt buộc')
})

export const reviewTT = yup.object().shape({
  feedback: yup.string().required('Phản hồi là bắt buộc'),
  rating: yup.number().required('Đánh giá là bắt buộc')
})

export const serviceSchema = yup.object().shape({
  pricePerHour: yup
    .number()
    .min(20000, 'Giá mỗi giờ tối thiểu là 20,000 VND')
    .transform((value, originalValue) => {
      // Trả về số nguyên luôn
      const formattedValue = parseFloat(originalValue.replace(/,/g, ''))
      // Nếu là chuỗi thì là 0
      return isNaN(formattedValue) ? 0 : formattedValue
    })

    .required('Giá là bắt buộc')
    .positive('Giá không thể là số âm'),
  title: yup.string().required('Phải nhập tiêu đề'),
  subject: yup
    .string()
    .required('Môn học là bắt buộc')
    .oneOf(
      [
        'Ngữ văn',
        'Toán học',
        'Vật lý',
        'Hóa học',
        'Sinh học',
        'Lịch sử',
        'Địa lý',
        'Giáo dục công dân',
        'Ngoại ngữ',
        'Tin học'
      ],
      'Môn học không hợp lệ'
    ),
  class: yup.string().oneOf(['10', '11', '12']).required('Chọn lớp'),
  description: yup.string().required('Phải nhập mô tả'),
  learningMethod: yup
    .string()
    .oneOf(
      ['Dạy trực tiếp(offline)', 'Dạy trực tuyến (online)'],
      'Phương thức học không hợp lệ'
    )
    .required('Hãy chọn phương thức học'),
  schedule: yup
    .array()
    .of(
      yup.object().shape({
        date: yup.string().required('Phải chọn ngày'),

        timeSlots: yup
          .array()
          .of(yup.string().required('Phải chọn ít nhất một khung giờ'))
          .min(1, 'Phải chọn ít nhất một khung giờ')
      })
    )
    .required('Phải thêm lịch học')
})

// này là mình export cái schema (đinhj dạng lỗi) của mình ra để qua bên Input bắt lỗi
export type SchemaResAT = yup.InferType<typeof schemaResAT>

export type Requestchema = yup.InferType<typeof requestSchema>
export type UserSchema = yup.InferType<typeof userSchema>
export type Schema = yup.InferType<typeof schema>
export type UpdateSchema = yup.InferType<typeof updateSchema>
export type UpdateTTSchema = yup.InferType<typeof updateTT>
//----------------------------------------------------------------
export type UpdateMajorTT = yup.InferType<typeof updateMajorTT>
export type UpdateProfileTT = yup.InferType<typeof updateProfileTT>
//-------------------
export type ReviewTT = yup.InferType<typeof reviewTT>
export type ServiceSchema = yup.InferType<typeof serviceSchema>
