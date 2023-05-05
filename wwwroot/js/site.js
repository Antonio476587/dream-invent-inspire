// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// BarbaJS code
barba.init({
  transitions: [{
    name: 'opacity-transition',
    leave(data) {
      return gsap.to(data.current.container, {
        opacity: 0,
        position: "absolute",
        width: "100%",
        top: 100
      });
    },
    enter(data) {
      return gsap.from(data.next.container, {
        opacity: 0,
        translateY: 100
      });
    }
  }]
});

// SignalR code
const connection = new signalR.HubConnectionBuilder().withUrl("/todohub").build();

window.addEventListener("submit", (e) => {
  connection.invoke("SendSignal");
});  

connection.on("ReloadPage", () => {
  location.reload();
});

connection.start().then(() => console.log("Conected"));