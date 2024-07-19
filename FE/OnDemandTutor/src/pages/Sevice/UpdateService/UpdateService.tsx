// import React, { useEffect } from 'react';
// import { useForm, Controller } from 'react-hook-form';
// import * as yup from 'yup';
// import { yupResolver } from '@hookform/resolvers/yup';
// import ScheduleItemForm from '../components/ScheduleItemForm';
// import { useMutation, useQuery } from '@tanstack/react-query';
// import { tutorApi } from '../../../api/tutor.api';
// import { toast } from 'react-toastify';

// const schema = yup.object().shape({
//   pricePerHour: yup.number().required('Phải nhập giá mỗi giờ').positive('Giá phải là số dương và lớn hơn 0'),
//   title: yup.string().required('Phải nhập tiêu đề'),
//   subject: yup.string().required('Phải chọn môn học'),
//   class: yup.string().required('Phải chọn lớp'),
//   description: yup.string().required('Phải nhập mô tả'),
//   learningMethod: yup.string().required('Phải chọn phương pháp học'),
//   schedule: yup.array().of(
//     yup.object().shape({
//       date: yup.string().required('Phải chọn ngày'),
//       timeSlots: yup.array().of(yup.string().required()).min(1, 'Phải chọn ít nhất một khung giờ')
//     })
//   ).required('Phải thêm lịch học')
// });

// type FormDataService = yup.InferType<typeof schema>;

// interface ServiceFormProps {
//   serviceId?: string; // Optional service ID for updating an existing service
// }

// export default function ServiceForm({ serviceId }: ServiceFormProps) {
//   const { control, handleSubmit, setValue, watch, formState: { errors }, reset } = useForm<FormDataService>({
//     resolver: yupResolver(schema),
//     defaultValues: {
//       pricePerHour: 0,
//       title: '',
//       subject: '',
//       class: '',
//       description: '',
//       learningMethod: '',
//       schedule: []
//     }
//   });

//   const formData = watch();
//   const [currentWeekStart, setCurrentWeekStart] = React.useState<Date>(new Date());

//   const subjects = ['Toán học', 'Văn học', 'Anh văn'];
//   const classes = ['11', '12', '10'];
//   const learningMethods = ['Online', 'Offline'];
//   const hours = [
//     '01:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30', '12:00',
//     '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00',
//     '17:30', '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00'
//   ];

//   const getDayOfWeek = (dateString: string) => {
//     const date = new Date(dateString);
//     const daysOfWeek = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'];
//     return daysOfWeek[date.getDay()];
//   };

//   const generateWeekDates = (startDate: Date) => {
//     const dates: Date[] = [];
//     const startDay = startDate.getDay();
//     const daysUntilMonday = startDay === 0 ? -6 : 1 - startDay;
//     const monday = new Date(startDate);
//     monday.setDate(startDate.getDate() + daysUntilMonday);

//     for (let i = 0; i < 7; i++) {
//       const date = new Date(monday);
//       date.setDate(monday.getDate() + i);
//       dates.push(date);
//     }
//     return dates;
//   };

//   const handlePreviousWeek = () => {
//     setCurrentWeekStart(prevDate => {
//       const newDate = new Date(prevDate);
//       newDate.setDate(prevDate.getDate() - 7);
//       return newDate;
//     });
//   };

//   const handleNextWeek = () => {
//     setCurrentWeekStart(prevDate => {
//       const newDate = new Date(prevDate);
//       newDate.setDate(prevDate.getDate() + 7);
//       return newDate;
//     });
//   };

//   const handleAddSchedule = () => {
//     setValue('schedule', [
//       ...formData.schedule,
//       { date: '', timeSlots: [] }
//     ]);
//   };

//   const handleRemoveSchedule = (index: number) => {
//     const updatedSchedule = [...formData.schedule];
//     updatedSchedule.splice(index, 1);
//     setValue('schedule', updatedSchedule);
//   };

//   const handleDateChange = (index: number, date: string) => {
//     const updatedSchedule = [...formData.schedule];
//     if (index >= 0 && index < updatedSchedule.length) {
//       updatedSchedule[index].date = date;
//       updatedSchedule[index].timeSlots = [];
//       setValue('schedule', updatedSchedule);
//     } else {
//       console.error('Index out of bounds');
//     }
//   };

//   const handleScheduleChange = (date: string, timeSlot: string) => {
//     const updatedSchedule = formData.schedule.map(item => {
//       if (item.date === date) {
//         const timeSlots = item.timeSlots || [];
//         const timeSlotIndex = timeSlots.indexOf(timeSlot);
//         if (timeSlotIndex > -1) {
//           timeSlots.splice(timeSlotIndex, 1);
//         } else {
//           timeSlots.push(timeSlot);
//         }
//         return { ...item, timeSlots };
//       }
//       return item;
//     });
//     setValue('schedule', updatedSchedule);
//   };

//   const weekDates = generateWeekDates(currentWeekStart);

//   const createServiceMutation = useMutation({
//     mutationFn: (body: FormDataService) => tutorApi.createService(body)
//   });

//   const updateServiceMutation = useMutation({
//     mutationFn: (body: FormDataService) => tutorApi.updateService(serviceId, body)
//   });

//   useEffect(() => {
    
//   }, [];

//   const onSubmit = (data: FormDataService) => {
//     if (serviceId) {
//       updateServiceMutation.mutate(data, {
//         onSuccess: (res: any) => {
//           console.log('Service updated successfully:', res);
//           toast.success(res.message);
//           reset();
//         },
//         onError: (errors) => {
//           console.log(errors);
//         }
//       });
//     } else {
//       createServiceMutation.mutate(data, {
//         onSuccess: (res: any) => {
//           console.log('Service created successfully:', res);
//           toast.success(res.message);
//           reset();
//         },
//         onError: (errors) => {
//           console.log(errors);
//         }
//       });
//     }
//   };

//   return (
//     <div className='w-5/6'>
//       <div className="container mx-auto p-4 w-4/6">
//         <h1 className="text-3xl font-bold mb-4">{serviceId ? 'Cập nhật dịch vụ' : 'Tạo dịch vụ'}</h1>
//         <form onSubmit={handleSubmit(onSubmit)} className="mx-auto bg-slate-50 rounded-lg shadow-lg p-5" noValidate>
//           <div className="mb-4">
//             <label className="block text-gray-700 text-sm font-bold mb-2">
//               Tiêu đề:
//               <Controller
//                 name="title"
//                 control={control}
//                 render={({ field }) => (
//                   <input type="text" {...field} placeholder='Tiêu đề' required className="form-input mt-1 mx-auto block w-4/6 p-2 border rounded-md" />
//                 )}
//               />
//             </label>
//             {errors.title && <p className="text-red-500 text-xs mt-1">{errors.title.message}</p>}
//           </div>
//           <div className='flex w-4/6 mx-auto gap-10'>
//             <div className="mb-4 w-full">
//               <label className=" text-gray-700 text-sm font-bold mb-2">
//                 <Controller
//                   name="subject"
//                   control={control}
//                   render={({ field }) => (
//                     <select {...field} required className="form-select mt-1 block w-full p-2 rounded-md border">
//                       <option value="">Chọn môn</option>
//                       {subjects.map((subject, index) => (
//                         <option key={index} value={subject}>{subject}</option>
//                       ))}
//                     </select>
//                   )}
//                 />
//               </label>
//               {errors.subject && <p className="text-red-500 text-xs mt-1">{errors.subject.message}</p>}
//             </div>
//             <div className="mb-4 w-full">
//               <label className=" text-gray-700 text-sm font-bold mb-2">
//                 <Controller
//                   name="class"
//                   control={control}
//                   render={({ field }) => (
//                     <select {...field} required className="form-select mt-1 block w-full p-2 rounded-md border">
//                       <option value="">Chọn lớp</option>
//                       {classes.map((cls, index) => (
//                         <option key={index} value={cls}>{cls}</option>
//                       ))}
//                     </select>
//                   )}
//                 />
//               </label>
//               {errors.class && <span className="text-red-500 text-xs mt-1">{errors.class.message}</span>}
//             </div>
//           </div>

//           <div className="mb-4 w-4/6 mx-auto pl-2">
//             <label className="text-gray-700 text-sm font-bold mb-2 flex gap-12">
//               <div>Phương thức học: </div>
//               <Controller
//                 name="learningMethod"
//                 control={control}
//                 render={({ field }) => (
//                   <div className="flex space-x-2">
//                     {learningMethods.map((method, index) => (
//                       <button
//                         key={index}
//                         type="button"
//                         onClick={() => field.onChange(method)}
//                         className={`bg-pink-200 hover:bg-pink-500 text-white font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline ${formData.learningMethod === method ? 'bg-pink-600' : ''}`}
//                       >
//                         {method}
//                       </button>
//                     ))}
//                   </div>
//                 )}
//               />
//             </label>
//             {errors.learningMethod && <span className="text-red-500 text-xs mt-1">{errors.learningMethod.message}</span>}
//           </div>
//           <div className="mb-4 w-4/6 mx-auto">
//             <label className="text-gray-700 text-sm font-bold mb-2 flex mt-auto">
//               <div className='my-auto'>Giá thuê mỗi tiếng: vd:200.000 VNĐ</div>
//               <Controller
//                 name="pricePerHour"
//                 control={control}
//                 render={({ field }) => (
//                   <div className='flex'>
//                     <input type="text" {...field} required className="form-input mt-1 block w-full p-2 rounded-md border " />
//                     <p className='my-auto'>VNĐ</p>
//                   </div>
//                 )}
//               />
//             </label>
//             {errors.pricePerHour && <p className="text-red-500 text-xs mt-1">{errors.pricePerHour.message}</p>}
//           </div>

//           <hr />

//           <div className="mb-4 w-4/6 mx-auto border ">
//             <label className="block text-gray-700 text-sm font-bold mb-2">Chọn lịch:</label>
//             <div className=''>
//                 {formData.schedule && formData.schedule.map((item, index) => (
//                 <ScheduleItemForm
//                     key={index}
//                     index={index}
//                     item={item}
//                     weekDates={weekDates}
//                     hours={hours}
//                     handleDateChange={handleDateChange}
//                     handleScheduleChange={handleScheduleChange}
//                     handleRemoveSchedule={handleRemoveSchedule}
//                     getDayOfWeek={getDayOfWeek}
//                     handlePreviousWeek={handlePreviousWeek}
//                     handleNextWeek={handleNextWeek}
//                     errors={errors.schedule && errors.schedule[index]}
//                 />
//                 ))}
//                 {errors.schedule && typeof errors.schedule === 'string' && <div className="text-red-500 text-xs mt-1">{errors.schedule}</div>}
//             </div>
            
//             <button type="button" onClick={handleAddSchedule} className="bg-green-500 hover:bg-green-600 text-white font-bold py-1 px-2 rounded focus:outline-none focus:shadow-outline mt-2">
//               Thêm ngày
//             </button>
//           </div>
//           <hr />
//           <div className="mt-4 w-2/3 mx-auto">
//             <label className="block text-gray-700 text-sm font-bold mb-2">
//               Mô tả về dịch vụ này:
//               <Controller
//                 name="description"
//                 control={control}
//                 render={({ field }) => (
//                   <textarea {...field} required className="form-textarea mt-1 block w-full p-2"></textarea>
//                 )}
//               />
//             </label>
//             {errors.description && <p className="text-red-500 text-xs mt-1">{errors.description.message}</p>}
//           </div>
//           <div className="mt-4">
//             <button type="submit" className="bg-green-500 hover:bg-green-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">{serviceId ? 'Cập nhật' : 'Tạo'}</button>
//           </div>
//         </form>
//       </div>
//     </div>
//   );
// };