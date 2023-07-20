<template>
    <h1>Employee Tracking Form</h1>
<form @submit.prevent="submitForm" v-if="!formSubmitted">
      <span>Select Project</span><br>
        <select v-model="selectedProject">
                    <option v-for="proj in projects" :value="proj">{{proj}}</option>
        </select>
      <br>
      <span>Select Employee</span><br>
      <select v-model="selectedEmployee">
                    <option v-for="emp in employees" :value="emp">{{emp}}</option>
      </select>
      <br>
      <span>Start Time</span><br>
      <input v-model=start type="datetime-local" id="start" name="start">
      <br>
      <span>End Time</span><br>
      <input v-model=end type="datetime-local" id="end" name="end">
      <br>
      <span>Work Description</span>
      <br>
      <input type="text" v-model=work id="work" name="work" placeholder="Work Description" minlength="8">
      <br>
      <input 
        class="submit" 
        type="submit" 
        value="Submit"
      >
    </form>
    <div v-if="formSubmitted">
      <h3>Form Submitted</h3>
      <p>Project: {{ selectedProject }}</p>
      <p>Employee: {{ selectedEmployee }}</p>
      <p>Start Time: {{ start }}</p>
      <p>End Time: {{ end }}</p>
      <p>Work Description: {{ work }}</p>
    </div>
</template>
<script>
 import axios from "axios";
 
 export default {
     data() {
         return {
             projects: [],
             selectedProject: "",
             employees: [],
             selectedEmployee: "",
             errors: [],
             start: "",
             end: "",
             work:"",
             formSubmitted: false
         };
     },
     
     methods: {
      submitForm: function () {
        const startTime = new Date(this.start)
        const endTime = new Date(this.end)
        if (startTime > endTime) {
          alert("Start time must be before end time")
          return
        }
        axios
        .post(`http://localhost:5160/projectitems`, {
          Name: this.selectedProject,
          EmployeeName: this.selectedEmployee,
          startTime: this.start,
          endTime: this.end,
          workDesc: this.work
        })
        this.formSubmitted = true
      }
    },

     // Pulls projects when the component is created.
     created() {
         axios
             .get(`http://localhost:5160/projectitems`)
             .then((response) => {
                 // JSON responses are automatically parsed
                 response.data.forEach((project) => {
                  if (!this.employees.includes(project.employeeName))
                     this.employees.push(project.employeeName);
                  if (!this.projects.includes(project.name))
                     this.projects.push(project.name);
                 });
                 console.log(this.projects);
             })
             .catch((e) => {
                 this.errors.push(e);
             });
     }
 };
</script>
<style>
h1 {
    /* margin-bottom: 50px; */
    background: #f2f2f2;
    color: rgb(135, 135, 135);
    font-weight: 500;
    padding-bottom: 60px;
}

body {
    /* font-family: "Open Sans", sans-serif; */
    background: #f2f2f2;
    margin-top: 80px;
    /* margin-bottom: 90px; */
    /* padding: 100px; */
    display: flex;
    justify-content: center;
    align-items: center;
    /* max-height: 100vh; */
}
form {
    align-items: center;
    padding: 10px;
    padding-left: 50px;
    padding-right: 50px;
    border: 2px solid black;
    border-radius: 15px;
    display: flex;
    flex-direction: column;
    justify-content: center;
  }


form > select {
    align-items: center;
    width: 240px;
    height: 32px;
    border: 2px solid #222;
    border-radius: 5px;
    margin: 10px;
}
form > input > option {
    
    align-items: center;
    width: 120px;
    border: 2px solid #222;
    border-radius: 5px;
    margin: 10px;
}
input 
{
    padding: 4px 8px;
    margin: 4px;
}

  span {
    font-size: 18px;
    margin: 4px;
    font-weight: 500;
    margin-left: 12px;
  }

  .submit {
    font-size: 15px;
    color : rgb(19, 18, 18);
    background: #f2f2f2;
    padding: 6px 12px;
    border: none;
    margin-left: 20px;
    cursor: pointer;
    border-radius: 5px;
    border: 2px solid black;
  }
#start {
    border: 2px solid #222;
    border-radius: 5px;
}
#end {
    border: 2px solid #222;
    border-radius: 5px;
}

#work {
    border: 2px solid #222;
    border-radius: 5px;
    width: 240px;
    height: 80px;
}



</style>